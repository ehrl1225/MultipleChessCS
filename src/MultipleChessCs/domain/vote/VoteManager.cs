namespace MultipleChessCs.Domain.Vote;
using System.Timers;

public class VoteManager
{
    private readonly Lock _lock = new(); 
    private TaskCompletionSource<Vote?>? _tcs;
    private readonly Dictionary<string, Vote> _votes = new();
    private HashSet<string> _voters = [];
    private Timer? _timer;

    public async Task<Vote?> StartVoteAsync(IEnumerable<string> voters, int seconds)
    {
        Task<Vote?> voteTask;
        lock (_lock)
        {
            if (_tcs != null)
            {
                _timer?.Stop();
                _tcs.TrySetResult(null);
            }
            _votes.Clear();
            _voters = new HashSet<string>(voters);
            _tcs = new TaskCompletionSource<Vote?>();
            voteTask = _tcs.Task;
            _timer = new Timer(seconds * 1000);
            _timer.AutoReset = false;
            _timer.Elapsed += (s, e) => FinishVote();
            _timer.Start();
        }
        return await voteTask;
    }

    public void CastVote(Vote vote)
    {
        lock (_lock)
        {
            if (!_voters.Contains(vote.PlayerName) || _tcs == null || _tcs.Task.IsCompleted)
                return;
            _votes[vote.PlayerName] = vote;

            if (_votes.Count >= _voters.Count)
            {
                FinishVote();
            }
        }
    }

    private void FinishVote()
    {
        lock (_lock)
        {
            if (_tcs == null) return;
            _timer?.Stop();
            _timer?.Dispose();
            Vote? result = AggregateResult();
            _tcs?.TrySetResult(result);
            _tcs = null;
        }
    }

    private Vote? AggregateResult()
    {
        if (_votes.Count == 0) return null;
        return _votes.Values
            .GroupBy( v => v.GetChoiceKey())
            .OrderByDescending(g => g.Count())
            .First()
            .First();
    }
    
}