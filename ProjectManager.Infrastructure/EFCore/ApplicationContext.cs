using Microsoft.EntityFrameworkCore;
using ProjectManager.Domain.Entities;
using ProjectManager.Infrastructure.Configurations;

namespace ProjectManager.Infrastructure.EFCore
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectParticipation> ProjectUsers { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<TeamParticipation> TeamUsers { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<SubTask> SubTasks { get; set; }
        public DbSet<TaskParticipation> TaskParticipations { get; set; }

        public ApplicationContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer("Server=DESKTOP-GPON2I3;Database=PMF;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.ApplyConfiguration(new TaskEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());

        }
    }
}
