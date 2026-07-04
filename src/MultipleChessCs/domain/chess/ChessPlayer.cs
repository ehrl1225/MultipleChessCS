namespace MultipleChessCs.Domain.Chess;
using Enum;



public class ChessPlayer(string username, string connectionId)
{
    public readonly string Username = username;
    public string ConnectionId {get; set;} = connectionId;
    public ChessTeam Team { get; set; } = ChessTeam.Viewer;
}