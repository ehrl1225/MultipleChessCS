using domain.Chess.ChessTeam;

namespace Domain.Chess.ChessPiece;


class ChessPiece
{
    private readonly int index;
    private ChessLocation initLocation;
    private ChessLocation location;
    private readonly ChessTeam team;
    private readonly ChessClass chessClass;
    private bool isDead;

    // for pawn
    private bool promoted;
    private ChessClass? promotedClass;
    private bool isFirstMove;


    public ChessPiece(
        int index,
        ChessLocation initLocation,
        ChessTeam team,
        ChessClass chessClass
    )
    {
        this.index = index;
        this.initLocation = initLocation;
        this.location = initLocation.copy();
        this.team = team;
        this.chessClass = chessClass;
        isDead = false;
        promoted = false;
        promotedClass = null;
    }

    public void promote(ChessClass promoteClass)
    {
        promoted = true;
        promotedClass = promoteClass;
    }

    public void move(int x, int y)
    {
        location.move(x, y);
    }

    public void kill()
    {
        isDead = true;
    }

    public void reset()
    {
        promoted = false;
        promotedClass = null;
        isDead = false;
    }

}