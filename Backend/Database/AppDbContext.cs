using Contracts.Dbo;
using Contracts.Dbo.Joint;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Database;

public class AppDbContext: IdentityDbContext<UserDbo, Microsoft.AspNetCore.Identity.IdentityRole<Guid>, Guid>
{
    public DbSet<RoomDbo> Rooms { get; set; }= default!;
    
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<RoomDbo>()
            .HasMany(g => g.Players)
            .WithMany(u => u.Rooms)
            .UsingEntity<UserRoomDbo>();
    }
}