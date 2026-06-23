using System.Runtime.InteropServices.ComTypes;
using System.Text.RegularExpressions;

namespace MultipleChessCs.Common;


using Domain.Chess.Enum;
using Domain.Chess;
using Domain.Player;
using Microsoft.AspNetCore.SignalR;


public class ChessHub(AuthService authService, ChessManager chessManager) : Hub<ChessHubInterface>
{
    private readonly AuthService _authService = authService;
    private readonly ChessManager _chessManager = chessManager;

    // user

    public async Task RequestRegister(string username, string password)
    {
        try
        {
            await _authService.Register(username, password);
            await Clients.Caller.HubResponse(HubAction.Register.ToInt(), true, "회원가입 성공");
        }
        catch (Exception)
        {
            await Clients.Caller.HubResponse(HubAction.Register.ToInt(), false, "회원가입 실패");
        }
    }

    public async Task RequestLogin(string username, string password)
    {
        bool result = await _authService.VerifyLogin(username, password);
        if (result == true)
        {
            await Clients.Caller.HubResponse(HubAction.Login.ToInt(), true, "로그인 성공");
            Context.Items["Username"] = username;
            return; 
        }

        await Clients.Caller.HubResponse(HubAction.Login.ToInt(), false, "로그인 실패");
    }

    // room

    public async Task RequestCreateRoom(string roomName, int maxPlayerCount)
    {
        if (Context.Items.TryGetValue("Username", out object? userObj) && userObj is string username)
        {
            if (Context.Items.TryGetValue("RoomId", out object? roomIdObj) && roomIdObj is string roomId) return;
            if (_chessManager.CreateRoom(username, roomName, maxPlayerCount))
            {
                await Clients.Caller.HubResponse(HubAction.CreateRoom.ToInt(), true, "방이 생성되었습니다.");
            }
            return;
        }

        await Clients.Caller.HubResponse(HubAction.CreateRoom.ToInt(), false, "방이 생성되지 않았습니다.");
    }

    public async Task RequestJoinRoom(string roomId)
    {
        if (Context.Items.TryGetValue("Username", out object? userObj) && userObj is string username)
        {
            ChessRoom? room = _chessManager.GetByRoomId(roomId);
            if (room == null)
            {
                await Clients.Caller.HubResponse(HubAction.JoinRoom.ToInt(), false, "방에 접속 실패했습니다.");
                return;
            }
            bool result = room.TryJoin(username);
            if (result)
            {
                await Clients.Caller.HubResponse(HubAction.JoinRoom.ToInt(), true, "방에 접속 성공했습니다.");
                return;
            }
        }
        await Clients.Caller.HubResponse(HubAction.JoinRoom.ToInt(), false, "방에 접속 실패했습니다.");
    }

    public async Task RequestDeleteRoom(string roomId)
    {
        if (Context.Items.TryGetValue("Username", out object? userObj) && userObj is string username)
        {
            bool result = _chessManager.DeleteRoom(roomId, username);
            if (result)
            {
                await Clients.Caller.HubResponse(HubAction.DeleteRoom.ToInt(), true, "방을 삭제했습니다.");
                return;
            }
        }
        await Clients.Caller.HubResponse(HubAction.DeleteRoom.ToInt(), false, "방 삭제 실패했습니다.");
    }

    public async Task RequestLeaveRoom(string roomId)
    {
        if (Context.Items.TryGetValue("Username", out object? userObj) && userObj is string username)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
            Context.Items.Remove("RoomId");
            await Clients.Caller.HubResponse(HubAction.LeaveRoom.ToInt(), true, "방을 나왔습니다.");
            return;
        }

        await Clients.Caller.HubResponse(HubAction.LeaveRoom.ToInt(), false, "방을 나오는데 실패했습니다.");
    }

    public async Task GetRoomList()
    {
        if (Context.Items.TryGetValue("Username", out object? userObj) && userObj is string username)
        {
            var chessRooms = _chessManager.GetChessRooms().Select(i => i.ToDto()).ToArray();
            await Clients.Caller.ChessRoomListResponse(chessRooms);
            return;
        }
        await Clients.Caller.Alert("로그인을 해야합니다.");
    }

    public async Task StartRoomGame(string roomId)
    {
        if (Context.Items.TryGetValue("Username", out object? userObj) && userObj is string username)
        {
            _chessManager.StartGame(roomId, username);
            await Clients.Group(roomId).GroupNotice("게임이 시작했습니다.");
            return;
        }
    }

    // team

    public async Task JoinTeam(string roomId, ChessTeam teamName)
    {
        await LeaveTeam(roomId);
        if (Context.Items.TryGetValue("Username", out object? userObj) && userObj is string username)
        {
            string groupName = $"{roomId}_{teamName}";
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).GroupNotice($"{username}님이 참여했습니다.");
            Context.Items["RoomId"] = roomId;
            Context.Items["TeamName"] = teamName;
            await Clients.Caller.HubResponse(HubAction.JoinTeam.ToInt(), true, "팀 참여 성공했습니다.");
            return;
        }
        await Clients.Caller.HubResponse(HubAction.JoinTeam.ToInt(), false, "팀 참여 실패했습니다.");
    }

    private async Task LeaveTeam(string roomId)
    {
        if (Context.Items.TryGetValue("Username", out object? userObj) && userObj is string username)
        {
            string blackTeam = $"{roomId}_{ChessTeam.Black}";
            string whiteTeam = $"{roomId}_{ChessTeam.White}";
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, blackTeam);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, whiteTeam);
            await Clients.Caller.HubResponse(HubAction.LeaveTeam.ToInt(), true, "팀에서 나갔습니다.");
            return;
        }
        await Clients.Caller.HubResponse(HubAction.LeaveTeam.ToInt(), false, "팀에서 나가지 못했습니다.");
    }

    // chat

    public async Task SendChat(ChatTarget chatTarget, string message)
    {
        switch (chatTarget)
        {
            case ChatTarget.Room:
                {
                    if (Context.Items.TryGetValue("Username", out object? userObj) && userObj is string username)
                    {
                        if (Context.Items.TryGetValue("RoomId", out object? roomIdObj) && roomIdObj is string roomId)
                        {
                            await Clients.Group(roomId).SendMessage(username, message);
                        }
                    }
                    break;  
                }

            case ChatTarget.Team:
                {
                    if (Context.Items.TryGetValue("Username", out object? userObj) && userObj is string username)
                    {
                        if (Context.Items.TryGetValue("RoomId", out object? roomIdObj) && roomIdObj is string roomId)
                        {
                            if (Context.Items.TryGetValue("TeamName", out object? teamNameObj) && teamNameObj is string teamName)
                            {
                                string groupName = $"{roomId}_{teamName}";
                                await Clients.Group(groupName).SendMessage(username, message);
                            }
                        }
                    }
                    break;
                }
            case ChatTarget.All:
                {
                    if (Context.Items.TryGetValue("Username", out object? userObj) && userObj is string username)
                    {
                        await Clients.All.SendMessage(username, message);
                    }
                    break;  
                }
            default:
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