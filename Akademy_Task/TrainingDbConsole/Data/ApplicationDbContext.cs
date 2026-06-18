using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using TrainingDbConsoleApp.Models;

namespace TrainingDbConsoleApp.Data
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
            });
        }
    }
}