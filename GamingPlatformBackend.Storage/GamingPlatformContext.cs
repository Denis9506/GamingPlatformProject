using GamingPlatformBackend.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace GamingPlatformBackend.Storage
{
    public class GamingPlatformContext : DbContext
    {
        public GamingPlatformContext(DbContextOptions<GamingPlatformContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Game>()
                .HasIndex(u => u.Name)
                .IsUnique();
            modelBuilder.Entity<User>()
          .HasOne(u => u.CurrentSession)
          .WithMany()  
          .HasForeignKey("CurrentSessionId"); 


            modelBuilder.Entity<GameSession>()
               .HasMany(gs => gs.Players)
               .WithMany(u => u.GameSessions)
               .UsingEntity(j => j.ToTable("GameSessionPlayers"));
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<GameSession> GameSessions { get; set; }
        public DbSet<Score> Scores { get; set; }
        public DbSet<Leaderboard> Leaderboard { get; set; }
    }
}
