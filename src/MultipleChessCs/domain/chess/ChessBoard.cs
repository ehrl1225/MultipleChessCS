namespace Chess.ChessBoard;
using Domain.Chess.ChessPiece;

class ChessBoard
{
    private ChessPiece[] pieces;

    public ChessBoard()
    {
        pieces = new ChessPiece[32];
    }
}
