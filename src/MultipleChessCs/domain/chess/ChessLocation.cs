namespace MultipleChessCs.Domain.Chess;

public class ChessLocation
{
    public int x {get; private set;}
    public int y {get; private set;}

    public ChessLocation(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public void Move(ChessLocation location)
    {
        x = location.x;
        y = location.y;
    }

    public void MoveX(int x)
    {
        this.x = x;
    }

    public void MoveXY(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public void MoveY(int y)
    {
        this.y = y;
    }

    public void AddX(int dx)
    {
        x += dx;
    }


    public void AddY(int dy)
    {
        y += dy;
    }

    public void AddXY(int dx, int dy)
    {
        x += dx;
        y += dy;
    }

    public ChessLocation Copy()
    {
        return new ChessLocation(x,y);
    }

    public bool IsInRange()
    {
        if ( x < 1 || x > 8) return false;
        if ( y < 1 || y > 8) return false;
        return true;
    }

    public bool Equals(ChessLocation other)
    {
        if (x != other.x) return false;
        if ( y != other.y) return false;
        return true;
    }
}