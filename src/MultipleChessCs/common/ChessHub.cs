using System.Runtime.InteropServices.Swift;

namespace MultipleChessCs.Common;


using Domain.Chess.Enum;
using Domain.Chess;
using Domain.Player;
using Microsoft.AspNetCore.SignalR;


public class ChessHub(AuthService authService, ChessManager chessManager) : Hub<ChessHubInterface>
{
    private readonly AuthService _authService = authService;
    private readonly ChessManager _chessManager = chessManager;
    private string? Username => Context.Items.TryGetValue("Username", out var u) && u is string s ? s : null;
    private string? RoomId => Context.Items.TryGetValue("RoomId", out var r) && r is string s ? s : null;
    private string? TeamName => Context.Items.TryGetValue("TeamName", out var t) && t is string s ? s : null;

    // util
    private async Task<string?> CheckLogin(HubAction action)
    {
        var username = Username;
        if (username == null)
        {
            await Clients.Caller.HubResponse(action, false, "로그인을 해야합니다.");
        }
        return username;
    }
    
    private async Task<string?> CheckRoom(HubAction action)
    {
        var username = await CheckLogin(action);
        if (username == null) return null;
        var room = _chessManager.GetByUsername(username);
        if (room == null)
        {
            await Clients.Caller.HubResponse(action, false, "방에 접속해야합니다.");
            return null;
        }
        return room.RoomId;
    }

    
    private async Task<string?> CheckTeam(HubAction action)
    {
        var teamName = TeamName;
        if (teamName == null)
            await Clients.Caller.HubResponse(action, false, "팀에 접속해야합니다.");
        return teamName;
    }
    
    // user

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
        var username = await CheckLogin(HubAction.Logout);
        if (username == null) return;
        var roomId = RoomId;
        if (roomId != null)
        {
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

    // room

    public async Task RequestCreateRoom(string roomName, int maxPlayerCount)
    {
        var username = await CheckLogin(HubAction.CreateRoom);
        var roomId = RoomId;
        if (username == null) return;

        if (roomId != null)
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
        var username = await CheckLogin(HubAction.JoinRoom);
        if (username == null) return;

        ChessRoom? room = _chessManager.GetByRoomId(roomId);
        if (room == null)
        {
            await Clients.Caller.HubResponse(HubAction.JoinRoom, false, "방에 접속 실패했습니다.");
            return;
        }
        bool result = room.TryJoin(username, Context.ConnectionId);
        if (result)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
            Context.Items["RoomId"] = roomId;
            await Clients.Caller.HubResponse(HubAction.JoinRoom, true, "방에 접속 성공했습니다.");
            return;
        }
        await Clients.Caller.HubResponse(HubAction.JoinRoom, false, "방에 접속 실패했습니다.");
    }

    public async Task RequestDeleteRoom(string roomId)
    {
        var username = await CheckLogin(HubAction.DeleteRoom);
        if (username == null) return;
        
        bool result = _chessManager.DeleteRoom(roomId, username);
        if (result)
        {
            await Clients.Group(roomId).HubResponse(HubAction.DeleteRoom, true, "방을 삭제했습니다.");
            return;
        }
        await Clients.Caller.HubResponse(HubAction.DeleteRoom, false, "방 삭제 실패했습니다.");
    }

    public async Task RequestLeaveRoom(string roomId)
    {
        var username = await CheckLogin(HubAction.LeaveRoom);
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
        var username = await CheckLogin(HubAction.GetRoomList);
        if (username == null) return;
        var chessRooms = _chessManager.GetChessRooms().Select(i => i.ToDto()).ToArray();
        await Clients.Caller.HubResponse(HubAction.GetRoomList, true, "방을 가져오는데 성공했습니다.");
        await Clients.Caller.ChessRoomListResponse(chessRooms);
    }

    public async Task StartRoomGame(string roomId)
    {
        var username = await CheckLogin(HubAction.StartGame);
        if (username == null) return;
        _chessManager.StartGame(roomId, username);
        await Clients.Group(roomId).GroupNotice("게임이 시작했습니다.");
    }

    // team

    public async Task JoinTeam(string roomId, ChessTeam teamName)
    {
        var username = await CheckLogin(HubAction.JoinTeam);
        if (username == null) return;
        var room = _chessManager.GetByRoomId(roomId);
        if (room == null)
        {
            await Clients.Caller.HubResponse(HubAction.JoinTeam, false, "방이 없습니다.");
            return;
        }
        await LeaveTeam(roomId);
        string groupName = $"{roomId}_{teamName}";
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        await Clients.Group(groupName).GroupNotice($"{username}님이 참여했습니다.");
        Context.Items["RoomId"] = roomId;
        Context.Items["TeamName"] = teamName;
        await Clients.Caller.HubResponse(HubAction.JoinTeam, true, "팀 참여 성공했습니다.");
    }
    
    private async Task LeaveTeam(string roomId)
    {
        string blackTeam = $"{roomId}_{ChessTeam.Black}";
        string whiteTeam = $"{roomId}_{ChessTeam.White}";
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, blackTeam);
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, whiteTeam);
        await Clients.Caller.HubResponse(HubAction.LeaveTeam, true, "팀에서 나갔습니다.");
    }

    // chat

    public async Task SendChat(ChatTarget chatTarget, string message)
    {
        var username = await CheckLogin(HubAction.SendChat);
        if (username == null) return;
        switch (chatTarget)
        {
            case ChatTarget.Room:
                {
                    var roomId = await CheckRoom(HubAction.SendChat);
                    if (roomId == null) return;
                    await Clients.Group(roomId).SendMessage(username, message);
                    break;
                }

            case ChatTarget.Team:
                {
                    var roomId = await CheckRoom(HubAction.SendChat);
                    if (roomId == null) return;
                    var teamName = await CheckTeam(HubAction.SendChat);
                    if (teamName == null) return;
                    string groupName = $"{roomId}_{teamName}";
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

    // etc

    public async Task Ping(string message)
    {
        Console.WriteLine($"Received : {message}");
        await Clients.Caller.Pong("Pong");
    }
}