using Domain.Chess.Enum.ChessTeam;

namespace Domain.Chess.ChessPlayer;


class ChessPlayer(string username)
{
    public readonly string _username = username;
    public ChessTeam? Team {get; private set;}

    public void SetTeam(ChessTeam? team)
    {
        Team = team;
    }
}