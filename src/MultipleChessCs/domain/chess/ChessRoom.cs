namespace MultipleChessCs.Domain.Chess;
using Vote;
using Enum;
using Board;


public class ChessRoom(string roomId, string admin, int maxPlayers)
{
    public readonly string RoomId = roomId;
    private ChessTeam _currentTurn = ChessTeam.White;
    private readonly ChessBoard _chessBoard = new();
    private readonly Dictionary<string, ChessPlayer> _players = [];
    private readonly VoteManager _voteManager = new();
    public int MaxPlayers { get; } = maxPlayers;
    private readonly Lock _lock = new();
    private readonly string _admin = admin;
    private bool _isStarted = false;
    

    public bool IsAdmin(string admin)
    {
        return _admin == admin;
    }

    public bool StartGame(string admin)
    {
        if (IsAdmin(admin))
        {
            lock (_lock)
            {
                _isStarted = true;
                return true;
            }
        }
        return false;
    }

    public void SwitchTurn()
    {
        if (_currentTurn == ChessTeam.White)
        {
            _currentTurn = ChessTeam.Black;
            return;
        }
        _currentTurn = ChessTeam.White;
    }

    public bool TryJoin(string playerName)
    {
        lock (_lock)
        {
            if (_isStarted) return false;
            if (_players.Count >= MaxPlayers)
            {
                return false;
            }
            if (_players.ContainsKey(playerName))
            {
                return false;
            }
            _players.Add(playerName, new ChessPlayer(playerName));
            return true;
        }
    }

    public bool TryExit(string playerName)
    {
        lock (_lock)
        {
            if (!_players.ContainsKey(playerName))
            {
                return false;
            }
            _players.Remove(playerName);
            return true;
        }
    }

    public async Task RunGameLoop()
    {
        while (_isStarted)
        {
            var voters = _players.Values
                .Where(p => p.Team == _currentTurn)
                .Select( p => p._username);
            var moveResult = await _voteManager.StartVoteAsync(voters, 60);

            if (moveResult is MoveVote move)
            {
                _chessBoard.ExecuteMove(move.From, move.To);
            }

        }
    }
}

