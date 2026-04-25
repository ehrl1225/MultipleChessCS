using Microsoft.EntityFrameworkCore;
using Moq;
using Domain.Player;


namespace MultipleChessCsTest;

public class ChessHubTests : HubTestBase
{
    private async Task<Player> CreateUser(string username, string password)
    {
        await Hub.RequestRegister(username, password);
        var player = await Db.Players.FirstOrDefaultAsync(p => p.Username == username);
        Assert.NotNull(player);
        return player;
    }

    [Fact]
    public async Task PingTest()
    {
        await Hub.Ping("Ping");
        MockCaller.Verify( c => c.Pong("Pong"), Times.Once);
    }

    [Fact]
    public async Task RegisterTest()
    {
        string username = "testUser";
        string password = "password123";
        await CreateUser(username, password);
        MockCaller.Verify( c => c.RegisterResponse(true, "성공"), Times.Once);
    }

    [Fact]
    public async Task LoginTest()
    {
        string username = "testUser";
        string password = "password123";
        await CreateUser(username, password);
        await Hub.RequestLogin(username, password);
        MockCaller.Verify( c => c.LoginResponse(true, "성공"), Times.Once);
    }

    [Fact]
    public async Task CreateRoomTest()
    {
        string username = "testUser";
        string password = "password123";
        await CreateUser(username, password);
        await Hub.RequestLogin(username, password);
        await Hub.RequestCreateRoom(10);
        MockCaller.Verify( c => c.Alert("방이 생성되었습니다."), Times.Once);
    }

    [Fact]
    public async Task CreateRoomTestFail()
    {
        await Hub.RequestCreateRoom(10);
        MockCaller.Verify( c => c.Alert("방이 생성되지 않았습니다."), Times.Once);
    }

    [Fact]
    public async Task JoinRoomTest()
    {
        
    }

    
}