namespace MultipleChessCs.Domain.Chess;
using Vote;
using Enum;


public class ChessRoom
{
    public readonly string _roomId;
    private ChessTeam currentTurn;
    private readonly ChessBoard _chessBoard;
    private readonly Dictionary<string, ChessPlayer> _players = [];
    private readonly VoteManager _voteManager;
    public int MaxPlayers { get; } = 10;
    private readonly Lock _lock = new();
    private readonly string _admin;
    private bool _isStarted = false;


    public ChessRoom(string roomId, string admin, int maxPlayers)
    {
        _roomId = roomId;
        _admin = admin;
        MaxPlayers = maxPlayers;
        currentTurn = ChessTeam.White;
        _chessBoard = new ChessBoard();
    }

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
        if (currentTurn == ChessTeam.White)
        {
            currentTurn = ChessTeam.Black;
            return;
        }
        currentTurn = ChessTeam.White;
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
                .Where(p => p.Team == currentTurn)
                .Select( p => p._username);
            var moveResult = await _voteManager.StartVoteAsync(voters, 60);

            if (moveResult is MoveVote move)
            {
                _chessBoard.ExecuteMove(move.From, move.To);
            }

        }
    }
}

