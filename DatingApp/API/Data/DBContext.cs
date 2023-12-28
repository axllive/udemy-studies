using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API;

public class DBContext : IdentityDbContext<AppUser, AppRole, int, 
            IdentityUserClaim<int>,
            AppUserRole, IdentityUserLogin<int>, 
            IdentityRoleClaim<int>, IdentityUserToken<int> >
{
    public DBContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<UserLike> Likes { get; set; }
    public DbSet<Message> Messages {get; set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder){
        
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AppUser>( entity => {
            entity.HasMany(ur => ur.UserRoles)
                    .WithOne(u => u.User)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
        } );

        modelBuilder.Entity<AppRole>( entity => {
            entity.HasMany(ur => ur.UserRoles)
                    .WithOne(u => u.Role)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();
        } );

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
