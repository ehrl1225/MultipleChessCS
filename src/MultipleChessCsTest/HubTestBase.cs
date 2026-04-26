namespace MultipleChessCsTest;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Moq;
using MultipleChessCs.Common;
using MultipleChessCs.Domain.Player;
using Microsoft.AspNetCore.Identity;
using dotenv.net;


public abstract class HubTestBase : IDisposable
{
    protected readonly AppDbContext Db;
    protected readonly ChessHub Hub;
    protected readonly Mock<IHubCallerClients<ChessHubInterface>> MockClients;
    protected readonly Mock<ChessHubInterface> MockCaller;
    protected readonly Mock<IGroupManager> MockGroups;
    protected readonly Mock<HubCallerContext> MockContext;

    private readonly IDbContextTransaction _transaction;

    protected HubTestBase()
    {
        DotEnv.Load();
        string dbPassword = Environment.GetEnvironmentVariable("PG_PASSWORD") ?? "default_password";
        string connectionString = $"Host=localhost;Database=chess;Username=postgres;Password={dbPassword}";
        var options = new DbContextOptionsBuilder<AppDbContext>()
        .UseNpgsql(connectionString)
        .Options;

        Db = new AppDbContext(options);
        _transaction = Db.Database.BeginTransaction();

        var hasher = new PasswordHasher<Player>();
        var authService = new AuthService(Db, hasher);
        var chessManager = new ChessManager();

        Hub = new ChessHub(authService, chessManager);
        
        MockClients = new Mock<IHubCallerClients<ChessHubInterface>>();
        MockCaller = new Mock<ChessHubInterface>();
        MockGroups = new Mock<IGroupManager>();
        MockContext = new Mock<HubCallerContext>();
        MockContext.Setup(c => c.Items).Returns(new Dictionary<object, object?>());
        

        MockClients.Setup(c => c.Caller).Returns(MockCaller.Object);

        Hub.Clients = MockClients.Object;
        Hub.Groups = MockGroups.Object;

        var contextProperty = typeof(Hub).GetProperty(nameof(Hub.Context));
        contextProperty?.SetValue(Hub, MockContext.Object);
        
    }

    public void Dispose()
    {
        _transaction.Rollback();
        _transaction.Dispose();
        Db.Dispose();
    }
}