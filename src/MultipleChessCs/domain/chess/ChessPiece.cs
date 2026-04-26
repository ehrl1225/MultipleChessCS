namespace MultipleChessCs.Domain.Chess;
using Enum;



class ChessPiece
{
    private readonly int index;
    private readonly ChessLocation initLocation;
    public ChessLocation location {get; private set;}
    public readonly ChessTeam team;
    public readonly ChessClass chessClass;
    public bool isDead {get; private set;}

    // for pawn
    public bool promoted {get; private set;}
    public ChessClass? promotedClass {get; private set;}
    // 0, 1, 2의 값을 가짐
    // 폰의 경우 앙파상을 확인하기 위해 한번 움직이면 다른 팀이 움직일 때도 카운트를 함
    public int moveCount {get; private set;}


    public ChessPiece(
        int index,
        ChessLocation initLocation,
        ChessTeam team,
        ChessClass chessClass
    ):this(index, initLocation.x, initLocation.y, team, chessClass)
    {}

    public ChessPiece(
        int index,
        int x,
        int y,
        ChessTeam team,
        ChessClass chessClass
    )
    {
        ChessLocation initLocation = new(x,y);
        this.index = index;
        this.initLocation = initLocation;
        location = initLocation.Copy();
        this.team = team;
        this.chessClass = chessClass;
        isDead = false;
        promoted = false;
        promotedClass = null;
        moveCount = 0;
    }

    public void promote(ChessClass promoteClass)
    {
        promoted = true;
        promotedClass = promoteClass;
    }

    public void move(ChessLocation location)
    {
        this.location.Move(location);
        if (moveCount < 2)
        {
            moveCount++;
        }
    }

    public void kill()
    {
        isDead = true;
    }

    private bool isInitLocation()
    {
        return initLocation.Equals(location);
    }

    public void reset()
    {
        promoted = false;
        promotedClass = null;
        isDead = false;
        location.Move(initLocation);
        moveCount = 0;
    }

}