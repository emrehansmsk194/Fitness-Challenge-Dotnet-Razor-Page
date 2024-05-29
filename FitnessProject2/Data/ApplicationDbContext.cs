using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FitnessProject2.Data;

public class ApplicationDbContext : IdentityDbContext
{


    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
     public DbSet<Challenge> Challenges { get; set; }
     public DbSet<Participation> Participations { get; set; }
     public DbSet<Leaderboard> Leaderboards { get; set; }
     public DbSet<UserDetail> UserDetails{ get; set; }
     public DbSet<UserRate> UserRates { get; set; }
     protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Participation>()
                .HasOne(p => p.Challenge)
                .WithMany(c => c.Participations)
                .HasForeignKey(p => p.ChallengeNumber)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Participation>()
                .HasOne(p => p.Userid)
                .WithMany()
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Leaderboard>()
                .HasOne(l => l.Challenge)
                .WithMany(c => c.Leaderboards)
                .HasForeignKey(l => l.ChallengeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Leaderboard>()
                .HasOne(l => l.User)
                .WithMany()
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }

}
