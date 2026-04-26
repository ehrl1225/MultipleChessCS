namespace MultipleChessCs.Domain.Vote;
using Chess;



public class MoveVote : Vote
{
    public ChessLocation From { get; }
    public ChessLocation To { get; }

    public MoveVote(string playerName, ChessLocation from, ChessLocation to) : base(playerName)
    {
        From = from;
        To = to;
    }
    
    public override string GetChoiceKey() => $"{From.x},{From.y},{To.x},{To.y}";
}