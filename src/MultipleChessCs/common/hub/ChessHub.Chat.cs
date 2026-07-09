using MultipleChessCs.Domain.Chess;

namespace MultipleChessCs.Common.Hub;


public partial class ChessHub
{
    public async Task SendChat(ChatTarget chatTarget, string message)
    {
        string? username = await CheckLogin(HubAction.SendChat);
        if (username == null) return;
        switch (chatTarget)
        {
            case ChatTarget.Room:
            {
                ChessRoom? room = await CheckRoom(HubAction.SendChat);
                if (room == null) return;
                await Clients.Group(room.RoomId).SendMessage(username, message);
                break;
            }

            case ChatTarget.Team:
            {
                ChessRoom? room = await CheckRoom(HubAction.SendChat);
                if (room == null) return;
                ChessPlayer? player = room.GetPlayer(username);
                if (player == null) return;
                string groupName = $"{room.RoomId}_{player.Team}";
                await Clients.Group(groupName).SendMessage(username, message);
                break;
            }
            case ChatTarget.All:
            {
                await Clients.All.SendMessage(username, message);
                break;  
            }
            default:
                await Clients.Caller.HubResponse(HubAction.SendChat, false, "메시지를 보내는데 실패했습니다.");
                break;
        }
    }
}