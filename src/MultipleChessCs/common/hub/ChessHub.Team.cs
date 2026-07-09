
namespace MultipleChessCs.Common.Hub;
using Domain.Chess;
using Domain.Chess.Enum;


public partial class ChessHub
{
    public async Task JoinTeam(ChessTeam teamName)
    {
        string? username = await CheckLogin(HubAction.JoinTeam);
        if (username == null) return;
        ChessRoom? joinedRoom = _chessManager.GetByUsername(username);
        if (joinedRoom == null)
        {
            await Clients.Caller.HubResponse(HubAction.JoinTeam, false, "들어간 방이 없습니다.");
            return;
        }
        bool result = joinedRoom.JoinTeam(username, teamName);
        if (!result) return;
        string roomId = joinedRoom.RoomId;
        await LeaveTeam(roomId);
        string groupName = $"{roomId}_{teamName}";
        
        await Clients.Group(groupName).GroupNotice($"{username}님이 참여했습니다.");
        await Clients.Caller.HubResponse(HubAction.JoinTeam, true, "팀 참여 성공했습니다.");
        await Clients.Group(roomId).ChessRoomDetailResponse(joinedRoom.ToDetailDto());
    }
    
    private async Task LeaveTeam(string roomId)
    {
        string blackTeam = $"{roomId}_{ChessTeam.Black}";
        string whiteTeam = $"{roomId}_{ChessTeam.White}";
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, blackTeam);
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, whiteTeam);
        await Clients.Caller.HubResponse(HubAction.LeaveTeam, true, "팀에서 나갔습니다.");
    }
}