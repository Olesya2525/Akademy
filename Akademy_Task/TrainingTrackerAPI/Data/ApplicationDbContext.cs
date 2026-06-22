using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TrainingTrackerAPI.Models;

namespace TrainingTrackerAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<TrainingProgram> TrainingPrograms { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<UserProgram> UserPrograms { get; set; }
        public DbSet<Statistics> Statistics { get; set; }
        public DbSet<DailyNorm> DailyNorms { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Настройка таблицы Users.
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(e => e.UserId);
                entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.FullName).IsRequired().HasMaxLength(100);
                entity.HasIndex(e => e.Username).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();
            });

            // Настройка таблицы TrainingPrograms.
            modelBuilder.Entity<TrainingProgram>(entity =>
            {
                entity.ToTable("TrainingPrograms");
                entity.HasKey(e => e.ProgramId);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Type).IsRequired().HasMaxLength(50);

                // ДОБАВИТЬ СВЯЗЬ С ПОЛЬЗОВАТЕЛЕМ
                entity.HasOne(p => p.User)
                      .WithMany()
                      .HasForeignKey(p => p.UserId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Настройка таблицы Exercises.
            modelBuilder.Entity<Exercise>(entity =>
            {
                entity.ToTable("Exercises");
                entity.HasKey(e => e.ExerciseId);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);

                entity.HasOne(e => e.Program)
                      .WithMany(p => p.Exercises)
                      .HasForeignKey(e => e.ProgramId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Настройка таблицы Activities.
            modelBuilder.Entity<Activity>(entity =>
            {
                entity.ToTable("Activities");
                entity.HasKey(e => e.ActivityId);
                entity.Property(e => e.DurationMinutes).IsRequired();
                entity.Property(e => e.Note).HasMaxLength(200);

                entity.HasOne(a => a.Exercise)
                      .WithMany(e => e.Activities)
                      .HasForeignKey(a => a.ExerciseId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(a => a.User)
                      .WithMany()  
                      .HasForeignKey(a => a.UserId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<UserProgram>(entity =>
            {
                entity.ToTable("UserPrograms");
                entity.HasKey(up => new { up.UserId, up.ProgramId });

                entity.HasOne(up => up.User)
                      .WithMany(u => u.UserPrograms)
                      .HasForeignKey(up => up.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(up => up.Program)
                      .WithMany(p => p.UserPrograms)
                      .HasForeignKey(up => up.ProgramId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // 6. Настройка Statistics
            modelBuilder.Entity<Statistics>(entity =>
            {
                entity.ToTable("Statistics");
                entity.HasKey(e => e.StatisticId);

                entity.HasOne(s => s.User)
                      .WithMany(u => u.Statistics)
                      .HasForeignKey(s => s.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(s => s.Exercise)
                      .WithMany(e => e.Statistics)
                      .HasForeignKey(s => s.ExerciseId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(s => new { s.UserId, s.ExerciseId }).IsUnique();
            });

            // 7. Настройка DailyNorm
            modelBuilder.Entity<DailyNorm>(entity =>
            {
                entity.ToTable("DailyNorms");
                entity.HasKey(e => e.NormId);

                entity.HasOne(d => d.User)
                      .WithMany(u => u.DailyNorms)
                      .HasForeignKey(d => d.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(d => new { d.UserId, d.NormDate }).IsUnique();
            });
        }
    }
}