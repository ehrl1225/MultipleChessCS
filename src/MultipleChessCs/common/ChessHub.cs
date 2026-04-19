namespace Common.ChessHub;

using System.Net.WebSockets;
using Common.ChatTarget;
using Common.ChessHubInterface;
using Common.ChessManager;
using Domain.Chess.Enum.ChessTeam;
using Domain.Chess.ChessRoom;
using Domain.Player.AuthService;
using Microsoft.AspNetCore.SignalR;

public class ChessHub : Hub<ChessHubInterface>
{
    private readonly AuthService _authService;
    private readonly ChessManager _chessManager;
    public ChessHub(AuthService authService,ChessManager chessManager)
    {
        _authService = authService;
        _chessManager = chessManager;
    }

    public async Task RequestRegister(string username, string password)
    {
        try
        {
            await _authService.Register(username, password);
            await Clients.Caller.RegisterResponse(true, "성공");
        }
        catch (Exception)
        {
            await Clients.Caller.RegisterResponse(false, "실패");
        }
    }

    public async Task RequestLogin(string username, string password)
    {
        bool result = await _authService.VerifyLogin(username, password);
        if (result == true)
        {
            await Clients.Caller.LoginResponse(true, "성공");
            Context.Items["Username"] = username;
            return;
        }
        await Clients.Caller.LoginResponse(false, "실패");
    }

    public async Task RequestLoginAsAnonymouse()
    {
        Context.Items["Username"] = "익명";
    }

    public async Task RequestJoinRoom(string roomId)
    {
        if (Context.Items.TryGetValue("Username", out object? userObj) && userObj is string username)
        {
            ChessRoom? room = _chessManager.GetByRoomId(roomId);
            if (room == null)
            {
                return;
            }
            bool result = room.TryJoin(username);

        }
    }

    public async Task JoinTeam(string roomId, ChessTeam teamName)
    {
        if (Context.Items.TryGetValue("Username", out object? userObj) && userObj is string username)
        {
            string groupName = $"{roomId}_{teamName}";
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).GroupNotice($"{username}님이 참여했습니다.");
            Context.Items["RoomId"] = roomId;
            Context.Items["TeamName"] = teamName;
            return;
        }
        await Clients.Caller.CallerMessage("로그인을 해야합니다.");
    }

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
        }
    }

    public async Task Ping(string message)
    {
        Console.WriteLine($"Received : {message}");
        await Clients.Caller.Pong("Pong");
    }

}