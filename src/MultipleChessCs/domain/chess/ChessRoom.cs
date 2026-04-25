namespace Domain.Chess.ChessRoom;
using Domain.Chess.ChessBoard;
using Domain.Chess.Enum.ChessTeam;
using Domain.Chess.ChessPlayer;


public class ChessRoom
{
    public readonly string _roomId;
    private ChessTeam currentTurn;
    private readonly ChessBoard chessBoard;
    private readonly Dictionary<string, ChessPlayer> _players = [];
    public int MaxPlayers {get; private set;} = 10;
    private readonly Lock _lock = new();
    private readonly string _admin;
    public bool IsStarted {get; private set;} = false;


    public ChessRoom(string roomId, string admin, int maxPlayers)
    {
        _roomId = roomId;
        _admin = admin;
        MaxPlayers = maxPlayers;
        currentTurn = ChessTeam.White;
        chessBoard = new ChessBoard();
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
                IsStarted = true;
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
            if (IsStarted) return false;
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
}

