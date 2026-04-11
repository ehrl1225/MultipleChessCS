namespace Domain.Chess.ChessRoom;
using Domain.Chess.ChessBoard;
using Domain.Chess.ChessTeam;



public class ChessRoom
{
    private readonly string _roomId;
    private ChessTeam currentTurn;
    private ChessBoard chessBoard;


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

}

