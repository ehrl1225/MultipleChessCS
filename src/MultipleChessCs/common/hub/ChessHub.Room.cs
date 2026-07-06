namespace MultipleChessCs.Common.Hub;
using Domain.Chess;
using Domain.Chess.Enum;

public partial class ChessHub
{
    public async Task RequestCreateRoom(string roomName, int maxPlayerCount)
    {
        string? username = await CheckLogin(HubAction.CreateRoom);
        if (username == null) return;
        
        ChessRoom? room = await CheckRoom(HubAction.CreateRoom);
        
        if (room != null)
        {
            await Clients.Caller.HubResponse(HubAction.CreateRoom, false, "이미 방에 접속해있습니다.");
            return;
        }

        if (_chessManager.CreateRoom(username, Context.ConnectionId, roomName, maxPlayerCount))
        {
            await Clients.Caller.HubResponse(HubAction.CreateRoom, true, "방이 생성되었습니다.");
            return;
        }

        await Clients.Caller.HubResponse(HubAction.CreateRoom, false, "방을 생성하는데 실패했습니다.");
    }

    public async Task RequestJoinRoom(string roomId)
    {
        string? username = await CheckLogin(HubAction.JoinRoom);
        if (username == null) return;

        ChessRoom? room = _chessManager.GetByRoomId(roomId);
        if (room == null)
        {
            await Clients.Caller.HubResponse(HubAction.JoinRoom, false, "방이 존재하지 않습니다.");
            return;
        }
        ChessRoom? joinedRoom = _chessManager.GetByUsername(username);
        if (joinedRoom != null)
        {
            if (joinedRoom.RoomId != roomId)
            {
                await Clients.Caller.HubResponse(HubAction.JoinRoom, false, "이미 접속한 방이 있습니다.");
                return;
            } 
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
            await Clients.Caller.HubResponse(HubAction.JoinRoom, true, "방에 접속 성공했습니다.");
            return;
        }
        
        bool result =_chessManager.JoinRoom(username, Context.ConnectionId, roomId);
        if (result)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
            await Clients.Caller.HubResponse(HubAction.JoinRoom, true, "방에 접속 성공했습니다.");
            return;
        }
        await Clients.Caller.HubResponse(HubAction.JoinRoom, false, "방에 접속 실패했습니다.");
    }

    public async Task RequestDeleteRoom()
    {
        string? username = await CheckLogin(HubAction.DeleteRoom);
        if (username == null) return;
        ChessRoom? room = await CheckRoom(HubAction.DeleteRoom);
        if (room == null) return;

        string roomId = room.RoomId;

        if (!room.IsAdmin(username))
        {
            await Clients.Caller.HubResponse(HubAction.DeleteRoom, false, "방장이 아닙니다.");
            return;
        }

        var teams = new []{ ChessTeam.Black, ChessTeam.White };
        foreach (ChessPlayer player in room.GetPlayers())
        {
            await Groups.RemoveFromGroupAsync(player.ConnectionId, roomId);
            foreach (ChessTeam team in teams)
            {
                string teamName = $"{roomId}_{team}";
                await Groups.RemoveFromGroupAsync(player.ConnectionId, teamName);
            }
        }
        
        bool result = _chessManager.DeleteRoom(roomId);
        if (result)
        {
            await Clients.Group(roomId).HubResponse(HubAction.DeleteRoom, true, "방을 삭제했습니다.");
            return;
        }
        await Clients.Caller.HubResponse(HubAction.DeleteRoom, false, "방 삭제 실패했습니다.");
    }

    public async Task RequestLeaveRoom(string roomId)
    {
        string? username = await CheckLogin(HubAction.LeaveRoom);
        if (username == null) return;
        
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
        var result = Context.Items.Remove("RoomId");
        if (!result)
        {
            await Clients.Caller.HubResponse(HubAction.LeaveRoom, false, "방을 나오는데 실패했습니다.");
            return;
        }
        await Clients.Caller.HubResponse(HubAction.LeaveRoom, true, "방을 나왔습니다.");
    }

    public async Task GetRoomList()
    {
        string? username = await CheckLogin(HubAction.GetRoomList);
        if (username == null) return;
        var chessRooms = _chessManager.GetChessRooms().Select(i => i.ToDto()).ToArray();
        await Clients.Caller.HubResponse(HubAction.GetRoomList, true, "방을 가져오는데 성공했습니다.");
        await Clients.Caller.ChessRoomListResponse(chessRooms);
    }

    public async Task GetRoomInfo()
    {
        string? username = await CheckLogin(HubAction.RoomInfo);
        if (username == null) return;
        ChessRoom? joinedRoom = _chessManager.GetByUsername(username);
        if (joinedRoom == null)
        {
            await Clients.Caller.HubResponse(HubAction.RoomInfo, false, "방의 정보를 가져오는데 실패했습니다.");
            return;
        }
        await Clients.Caller.HubResponse(HubAction.RoomInfo, true, "방의 정보를 가져오는데 성공했습니다.");
        await Clients.Caller.ChessRoomDetailResponse(joinedRoom.ToDetailDto());
    }

    public async Task StartRoomGame(string roomId)
    {
        string? username = await CheckLogin(HubAction.StartGame);
        if (username == null) return;
        _chessManager.StartGame(roomId, username);
        await Clients.Group(roomId).GroupNotice("게임이 시작했습니다.");
    }
}