using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;


namespace MultipleChessCsTest;

public class ChessHubTests : HubTestBase
{
    [Fact]
    public async Task RegisterTest()
    {
        string username = "testUser";
        string password = "password123";
        await Hub.RequestRegister(username, password);
        var player = await Db.Players.FirstOrDefaultAsync(p => p.Username == username);
        Assert.NotNull(player);
        MockCaller.Verify( c => c.RegisterResponse(true, "성공"), Times.Once);
    }

    [Fact]
    public async Task LoginTest()
    {
        string username = "testUser";
        string password = "password123";
        await Hub.RequestRegister(username, password);
        var player = await Db.Players.FirstOrDefaultAsync(p => p.Username == username);
        Assert.NotNull(player);
        await Hub.RequestLogin(username, password);
        MockCaller.Verify( c => c.LoginResponse(true, "성공"), Times.Once);
    }
}