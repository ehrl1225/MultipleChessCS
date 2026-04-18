using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Moq;
using Common.ChessHub;
using Common.ChessHubInterface;
using Domain.Player.AuthService;
using Common.ChessManager;
using Microsoft.AspNetCore.Identity;
using Domain.Player;
using dotenv.net;

namespace MultipleChessCsTest;


public abstract class HubTestBase
{
    protected readonly AppDbContext Db;
    protected readonly ChessHub Hub;

    private IDbContextTransaction? _transaction;

    protected HubTestBase()
    {
        
    }
}