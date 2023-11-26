using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API;

public class DBContext : DbContext
{
    public DBContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<AppUser> Users { get; set; }
    public DbSet<UserLike> Likes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder){
        
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserLike>( entity =>
        {
            entity.HasKey( k => new {k.SoucerUserId, k.TargetUserId} );

            entity.HasOne(s => s.SoucerUser)
                    .WithMany(l => l.LikedUsers)
                    .HasForeignKey(s => s.SoucerUserId)
                    .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(s => s.TargetUser)
                    .WithMany(l => l.LikedByUsers)
                    .HasForeignKey(s => s.TargetUserId)
                    .OnDelete(DeleteBehavior.NoAction);
        });
    }
}
