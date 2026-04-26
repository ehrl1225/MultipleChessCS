namespace MultipleChessCs.Domain.Chess;
using Board;

class ChessThreat
{
    public ChessPiece AttackChessPiece {get; private set;}
    public List<ChessPiece> BlockChessPieces {get; private set;}
    public bool Blocked {get; private set;}
    public ChessLocation TargetLocation{get; private set;}

    public ChessThreat(
        ChessPiece attackChessPiece, 
        List<ChessPiece> blockChessPieces,
        bool blocked, 
        ChessLocation targetLocation
        )
    {
        AttackChessPiece= attackChessPiece;
        BlockChessPieces = blockChessPieces;
        Blocked = blocked;
        TargetLocation = targetLocation;
    }
}