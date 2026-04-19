namespace Domain.Chess.ChessRoom;
using Domain.Chess.ChessBoard;
using Domain.Chess.Enum.ChessTeam;
using Domain.Chess.ChessPlayer;


public class ChessRoom
{
    private readonly string _roomId;
    private ChessTeam currentTurn;
    private ChessBoard chessBoard;
    private readonly Dictionary<string, ChessPlayer> _players = new();
    private int MaxPlayers = 10;
    private readonly object _lock = new();


    public ChessRoom(string roomId)
    {
        _roomId = roomId;
        currentTurn = ChessTeam.White;
        chessBoard = new ChessBoard();
    }

    public void switchTurn()
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

