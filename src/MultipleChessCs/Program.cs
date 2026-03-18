using Microsoft.AspNetCore.Identity;
using Domain.Player;
using Domain.Player.AuthService;
using Common.ChessManager;
namespace Main;

class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        builder.Services.AddSignalR();
        builder.Services.AddDbContext<AppDbContext>();
        builder.Services.AddScoped<IPasswordHasher<Player>, PasswordHasher<Player>>();
        builder.Services.AddScoped<AuthService>();
        builder.Services.AddSingleton<ChessManager>();

        WebApplication app = builder.Build();

        app.Run();
    }
}
    
