namespace MultipleChessCs.Common.Hub;


using Domain.Chess.Enum;
using Domain.Chess;
using Domain.Player;
using Microsoft.AspNetCore.SignalR;


public partial class ChessHub(AuthService authService, ChessManager chessManager) : Hub<ChessHubInterface>
{
    private readonly AuthService _authService = authService;
    private readonly ChessManager _chessManager = chessManager;
    private string? Username => Context.Items.TryGetValue("Username", out var u) && u is string s ? s : null;
    private string? TeamName => Context.Items.TryGetValue("TeamName", out var t) && t is string s ? s : null;

    // util
    private async Task<string?> CheckLogin(HubAction action)
    {
        string? username = Username;
        if (username == null)
        {
            await Clients.Caller.HubResponse(action, false, "로그인을 해야합니다.");
        }
        return username;
    }
    
    private async Task<ChessRoom?> CheckRoom(HubAction action)
    {
        string? username = await CheckLogin(action);
        if (username == null) return null;
        ChessRoom? room = _chessManager.GetByUsername(username);
        if (room != null) return room;
        
        await Clients.Caller.HubResponse(action, false, "방에 접속해야합니다.");
        return null;
    }

    
    private async Task<string?> CheckTeam(HubAction action)
    {
        string? teamName = TeamName;
        if (teamName == null)
            await Clients.Caller.HubResponse(action, false, "팀에 접속해야합니다.");
        return teamName;
    }
}