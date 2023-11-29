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
    public DbSet<Message> Messages {get; set;}

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

        modelBuilder.Entity<Message>( entity =>
        {
            entity.HasKey( k => k.Id );

            entity.HasOne(s => s.Sender)
                    .WithMany(l => l.MessagesSent)
                    .HasForeignKey(s => s.SenderId)
                    .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(s => s.Recipient)
                    .WithMany(l => l.MessagesReceived)
                    .HasForeignKey(s => s.RecipientId)
                    .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
