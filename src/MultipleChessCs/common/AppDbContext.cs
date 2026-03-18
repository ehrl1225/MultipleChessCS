using Microsoft.EntityFrameworkCore;
using Domain.Player;


public class AppDbContext : DbContext{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }

    public DbSet<Player> Players => Set<Player>();
    
}