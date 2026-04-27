namespace MultipleChessCs.Domain.Vote;
using MultipleChessCs.Domain.Chess.Enum;


public class PromotionVote : Vote
{
    public ChessClass PromotionTo { get; }

    public PromotionVote(string playerName, ChessClass promotionTo) : base(playerName)
    {
        PromotionTo = promotionTo;
    }
    public override string GetChoiceKey() => PromotionTo.ToString();
}