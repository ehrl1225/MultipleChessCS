namespace MultipleChessCs.Domain.Vote;

public abstract class Vote(string playerName)
{
    public string PlayerName { get; protected set; } = playerName;
    public abstract string GetChoiceKey();
}