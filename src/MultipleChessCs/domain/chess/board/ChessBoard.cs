namespace MultipleChessCs.Domain.Chess.Board;

using Enum;
using Rules;

public class ChessBoard
{
    private ChessPiece?[,] board;
    private ChessPiece[] pieces;

    public ChessBoard()
    {
        board = new ChessPiece?[8, 8];

        pieces = new ChessPiece[32];
        InitChessPieces();
    }

    private void InitChessPieces()
    {
        int x;
        int whiteY = 2;
        int blackY = 7;
        int whiteIndex = 0;
        int blackIndex = 16;
        ChessPiece whiteChessPiece;
        ChessPiece blackChessPiece;

        // y = 2, x = 1 ~ 9, index = 0 ~ 7 white pawn
        // y = 7, x = 1 ~ 9, index = 16 ~ 23 black pawn
        for (x = 1; x < 9; x++)
        {
            whiteChessPiece = new(
                whiteIndex,
                x,
                whiteY,
                ChessTeam.White,
                ChessClass.PAWN
            );
            pieces[whiteIndex] = whiteChessPiece;
            board[whiteY-1, x-1] = whiteChessPiece;
            whiteIndex++;

            blackChessPiece = new(
                blackIndex,
                x,
                blackY,
                ChessTeam.Black,
                ChessClass.PAWN
            );
            pieces[blackIndex] = blackChessPiece;
            board[blackY-1, x-1] = blackChessPiece;
            blackIndex++;
        }

        whiteY = 1;
        blackY = 8;
        // y = 1, x = 1, 8, index = 8, 9 white rook
        // y = 8, x = 1, 8, index = 24, 25 black rook
        for (x = 1; x<9; x += 7)
        {
            whiteChessPiece = new(
                whiteIndex,
                x,
                whiteY,
                ChessTeam.White,
                ChessClass.ROOK
            );
            pieces[whiteIndex] = whiteChessPiece;
            board[whiteY-1, x-1] = whiteChessPiece;
            whiteIndex++;

            blackChessPiece = new(
                blackIndex,
                x,
                blackY,
                ChessTeam.Black,
                ChessClass.ROOK
            );
            pieces[blackIndex] = blackChessPiece;
            board[blackY-1, x-1] = blackChessPiece;
            blackIndex++;
        }

        // y = 1, x = 2, 7, index = 10, 11 white knight
        // y = 8, x = 2, 7, index = 26, 27 black knight
        for (x= 2; x<8; x += 5)
        {
            whiteChessPiece = new(
                whiteIndex,
                x,
                whiteY,
                ChessTeam.White,
                ChessClass.KNIGHT
            );
            pieces[whiteIndex] = whiteChessPiece;
            board[whiteY-1, x-1] = whiteChessPiece;
            whiteIndex++;

            blackChessPiece = new(
                blackIndex,
                x,
                blackY,
                ChessTeam.Black,
                ChessClass.KNIGHT
            );
            pieces[blackIndex] = blackChessPiece;
            board[blackY-1, x-1] = blackChessPiece;
            blackIndex++;
        }

        // y = 1, x = 3, 6, index = 12, 13 white bishop
        // y = 8, x = 3, 6, index = 28, 29 black bishop
        for (x= 3; x<7; x += 3)
        {
            whiteChessPiece = new(
                whiteIndex,
                x,
                whiteY,
                ChessTeam.White,
                ChessClass.BISHOP
            );
            pieces[whiteIndex] = whiteChessPiece;
            board[whiteY-1, x-1] = whiteChessPiece;
            whiteIndex++;

            blackChessPiece = new(
                blackIndex,
                x,
                blackY,
                ChessTeam.Black,
                ChessClass.BISHOP
            );
            pieces[blackIndex] = blackChessPiece;
            board[blackY-1, x-1] = blackChessPiece;
            blackIndex++;
        }

        // y = 1, x = 5, index = 14 white king
        // y = 8, x = 5, index = 30 black king
        x=5;
        whiteChessPiece = new(
            whiteIndex,
            x,
            whiteY,
            ChessTeam.White,
            ChessClass.KING
        );
        pieces[whiteIndex] = whiteChessPiece;
        board[whiteY-1, x-1] = whiteChessPiece;
        whiteIndex++;

        blackChessPiece = new(
            blackIndex,
            x,
            blackY,
            ChessTeam.Black,
            ChessClass.KING
        );
        pieces[blackIndex] = blackChessPiece;
        board[blackY-1, x-1] = blackChessPiece;
        blackIndex++;

        // y = 1, x = 4, index = 15 white queen
        // y = 8, x = 4, index = 31 black queen
        x=4;
        whiteChessPiece = new(
            whiteIndex,
            x,
            whiteY,
            ChessTeam.White,
            ChessClass.QUEEN
        );
        pieces[whiteIndex] = whiteChessPiece;
        board[whiteY-1, x-1] = whiteChessPiece;

        blackChessPiece = new(
            blackIndex,
            x,
            blackY,
            ChessTeam.Black,
            ChessClass.QUEEN
        );
        pieces[blackIndex] = blackChessPiece;
        board[blackY-1, x-1] = blackChessPiece;
    }

    public ChessPiece? GetByLocation(ChessLocation location)
    {
        ChessPiece? chessPiece = board[location.y-1, location.x-1];
        return chessPiece;
    }

    public ChessPiece? GetByIndex(int index)
    {
        if (index < 0 || index >= 32) return null;
        return pieces[index];
    }

    public void ExecuteMove(ChessLocation from, ChessLocation to)
    {
        ChessPiece? fromChessPiece = GetByLocation(from);
        if (fromChessPiece == null)
        {
            return;
        }
        board[from.y-1, from.x-1] = null;
        ChessPiece? toChessPiece = GetByLocation(to);
        if (toChessPiece != null) toChessPiece.kill();
        board[to.y-1, to.x-1] = fromChessPiece;
    }

}
