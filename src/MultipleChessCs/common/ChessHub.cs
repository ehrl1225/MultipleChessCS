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

    // room

    public async Task RequestCreateRoom(int maxPlayerCount)
    {
        if (Context.Items.TryGetValue("Username", out object? userObj) && userObj is string username)
        {
            if (Context.Items.TryGetValue("RoomId", out object? roomIdObj) && roomIdObj is string roomId) return;
            if (_chessManager.CreateRoom(username, maxPlayerCount))
            {
                await Clients.Caller.Alert("방이 생성되었습니다.");
            }
            return;
        }
        await Clients.Caller.Alert("방이 생성되지 않았습니다.");

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
            if (result) await Clients.Caller.Alert("방에 접속했습니다.");
        }
        await Clients.Caller.Alert("방에 접속 실패했습니다.");
    }

    public async Task RequestDeleteRoom(string roomId)
    {
        if (Context.Items.TryGetValue("Username", out object? userObj) && userObj is string username)
        {
            bool result = _chessManager.DeleteRoom(roomId, username);
            if (result) await Clients.Caller.Alert("방을 삭제했습니다.");
        }
        await Clients.Caller.Alert("방 삭제 실패했습니다.");
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
            return;
        }
    }

    // team

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
        await Clients.Caller.Alert("로그인을 해야합니다.");
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
        }
    }

    // etc

    public async Task Ping(string message)
    {
        Console.WriteLine($"Received : {message}");
        await Clients.Caller.Pong("Pong");
    }

}