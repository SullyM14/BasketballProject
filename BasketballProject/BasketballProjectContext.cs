using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BasketballProject
{
    public partial class BasketballProjectContext : DbContext
    {
        public BasketballProjectContext()
        {
        }

        public BasketballProjectContext(DbContextOptions<BasketballProjectContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Nbateams> Nbateams { get; set; }
        public virtual DbSet<Players> Players { get; set; }
        public virtual DbSet<UserTeamPlayers> UserTeamPlayers { get; set; }
        public virtual DbSet<UserTeams> UserTeams { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BasketballProject;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Nbateams>(entity =>
            {
                entity.HasKey(e => e.NbateamId)
                    .HasName("PK__tmp_ms_x__B5C90A5CA78F9230");

                entity.ToTable("NBATeams");

                entity.Property(e => e.NbateamId).HasColumnName("NBATeamId");

                entity.Property(e => e.Conference).HasMaxLength(50);

                entity.Property(e => e.TeamName)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Players>(entity =>
            {
                entity.HasKey(e => e.PlayerId)
                    .HasName("PK__tmp_ms_x__4A4E74C806DD4D1E");

                entity.Property(e => e.Apg)
                    .HasColumnName("APG")
                    .HasColumnType("decimal(18, 1)");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Position)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Ppg)
                    .HasColumnName("PPG")
                    .HasColumnType("decimal(18, 1)");

                entity.Property(e => e.Price).HasColumnType("decimal(18, 1)");

                entity.Property(e => e.Rpg)
                    .HasColumnName("RPG")
                    .HasColumnType("decimal(18, 1)");

                entity.Property(e => e.TeamId).HasColumnName("TeamID");

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.Players)
                    .HasForeignKey(d => d.TeamId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TeamID");
            });

            modelBuilder.Entity<UserTeamPlayers>(entity =>
            {
                entity.HasOne(d => d.Player)
                    .WithMany(p => p.UserTeamPlayers)
                    .HasForeignKey(d => d.PlayerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserTeamPlayers_ToPlayers");

                entity.HasOne(d => d.UserTeam)
                    .WithMany(p => p.UserTeamPlayers)
                    .HasForeignKey(d => d.UserTeamId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserTeamPlayers_ToUserTeam");
            });

            modelBuilder.Entity<UserTeams>(entity =>
            {
                entity.HasKey(e => e.UserTeamId)
                    .HasName("PK__tmp_ms_x__9ADF80B25A75C563");

                entity.Property(e => e.Budget).HasColumnType("decimal(18, 1)");

                entity.Property(e => e.TeamName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserTeams)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserTeams_ToUsers");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK__tmp_ms_x__1788CC4CADAB4A6B");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
