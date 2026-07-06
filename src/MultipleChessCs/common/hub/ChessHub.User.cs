namespace MultipleChessCs.Common.Hub;
using Domain.Chess;

public partial class ChessHub
{
    public async Task RequestRegister(string username, string password)
    {
        try
        {
            await _authService.Register(username, password);
            await Clients.Caller.HubResponse(HubAction.Register, true, "회원가입 성공");
        }
        catch (Exception)
        {
            await Clients.Caller.HubResponse(HubAction.Register, false, "회원가입 실패");
        }
    }

    public async Task RequestLogin(string username, string password)
    {
        bool result = await _authService.VerifyLogin(username, password);
        if (result)
        {
            await Clients.Caller.HubResponse(HubAction.Login, true, "로그인 성공");
            Context.Items["Username"] = username;
            return; 
        }

        await Clients.Caller.HubResponse(HubAction.Login, false, "로그인 실패");
    }

    public async Task Logout()
    {
        string? username = await CheckLogin(HubAction.Logout);
        if (username == null) return;
        ChessRoom? room = await CheckRoom(HubAction.Logout);
        if (room != null)
        {
            string roomId = room.RoomId;
            
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
            Context.Items.Remove(roomId);
            var teamName = TeamName;
            if (teamName != null)
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"{roomId}_{teamName}");
                Context.Items.Remove(teamName);
            }
        }
        

        await Clients.Caller.HubResponse(HubAction.Logout, true, "로그아웃 했습니다.");
    }
}