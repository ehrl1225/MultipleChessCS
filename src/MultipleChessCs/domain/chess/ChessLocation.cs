

class ChessLocation
{
    private int x;
    private int y;

    public ChessLocation(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public void move(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public ChessLocation copy()
    {
        return new ChessLocation(x,y);
    }
}