namespace MultipleChessCs.Domain.Player;
using Microsoft.AspNetCore.Identity;
using Common;

using Domain.Player;
using Microsoft.EntityFrameworkCore;

public class AuthService(AppDbContext db, IPasswordHasher<Player> hasher)
{
    private readonly AppDbContext _db = db;
    private readonly IPasswordHasher<Player> _hasher = hasher;

    public async Task Register(string username, string password)
    {
        if (await IsUsernameTaken(username))
        {
            throw new Exception("이미 사용 중인 아이디입니다.");
        }
        Player player = new Player {Username = username};
        player.PasswordHash = _hasher.HashPassword(player, password);
        _db.Players.Add(player);
        await _db.SaveChangesAsync();
    }

    public async Task<bool> VerifyLogin(string username, string password)
    {
        Player? player = await _db.Players.FirstOrDefaultAsync(p => p.Username == username);
        if (player == null) return false;
        PasswordVerificationResult result = _hasher.VerifyHashedPassword(player, player.PasswordHash, password);
        return result == PasswordVerificationResult.Success;
    }

    public async Task<bool> IsUsernameTaken(string username)
    {
        return await _db.Players.AnyAsync(p => p.Username == username);
    }
}