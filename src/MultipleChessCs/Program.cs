namespace MultipleChessCs;
using MultipleChessCs.Domain.Chess.Rules;
using Microsoft.AspNetCore.Identity;
using Domain.Player;
using Common;
using Common.Hub;
using dotenv.net;
using Microsoft.EntityFrameworkCore;


class Program
{
    public static void Main(string[] args)
    {
        DotEnv.Load();
        string dbPassword = Environment.GetEnvironmentVariable("PG_PASSWORD") ?? "default_password";
        string connectionString = $"Host=localhost;Database=chess;Username=postgres;Password={dbPassword}";

        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        builder.Services.AddSignalR();
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString));
        builder.Services.AddScoped<IPasswordHasher<Player>, PasswordHasher<Player>>();
        builder.Services.AddScoped<AuthService>();
        builder.Services.AddSingleton<ChessRules>();
        builder.Services.AddSingleton<ChessManager>();
        
        Console.WriteLine("빌드 시작");
        WebApplication app = builder.Build();
        app.MapHub<ChessHub>("/chess_hub");

        Console.WriteLine("실행");
        app.Run();
    }
}
    
