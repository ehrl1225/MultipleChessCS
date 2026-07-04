using MultipleChessCs.Domain.Chess.Rules;

namespace MultipleChessCs.Domain.Chess;
using Vote;
using Enum;
using Board;


public class ChessRoom(string roomId,string roomName,  string admin, int maxPlayers, ChessRules chessRules)
{
    public readonly string RoomId = roomId;
    public readonly string RoomName = roomName;
    private ChessTeam _currentTurn = ChessTeam.White;
    private readonly ChessBoard _chessBoard = new();
    private readonly Dictionary<string, ChessPlayer> _players = [];
    private readonly VoteManager _voteManager = new();
    public int MaxPlayers { get; } = maxPlayers;
    private readonly Lock _lock = new();
    public readonly string Admin = admin;
    public bool IsStarted { get; private set; } = false;
    private readonly ChessRules _chessRules = chessRules;

    public int GetPlayerCount()
    {
        return _players.Count;
    }

    public bool IsAdmin(string admin)
    {
        return Admin == admin;
    }

    public List<ChessPlayer> GetPlayers()
    {
        return _players.Values.ToList();
    }

    public ChessPlayer? GetPlayer(string playerName)
    {
        return _players.GetValueOrDefault(playerName);
    }

    public bool KickPlayer(string playerName)
    {
        lock (_lock)
        {
            return _players.Remove(playerName);
        }
    }

    public bool StartGame(string admin)
    {
        if (IsAdmin(admin))
        {
            lock (_lock)
            {
                IsStarted = true;
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

    public bool TryJoin(string playerName, string connectionId)
    {
        lock (_lock)
        {
            if (IsStarted) return false;
            if (_players.Count >= MaxPlayers)
            {
                return false;
            }
            if (_players.ContainsKey(playerName))
            {
                return false;
            }
            _players.Add(playerName, new ChessPlayer(playerName, connectionId));
            return true;
        }
    }

    public bool IsInRoom(string playerName)
    {
        return _players.ContainsKey(playerName);
    }

    public async Task RunGameLoop()
    {
        while (IsStarted)
        {
            var voters = _players.Values
                .Where(p => p.Team == _currentTurn)
                .Select( p => p.Username);
            var moveResult = await _voteManager.StartVoteAsync(voters, 60);

            if (moveResult is MoveVote move)
            {
                _chessBoard.ExecuteMove(move.From, move.To);
            }
            
            SwitchTurn();
        }
    }
}

