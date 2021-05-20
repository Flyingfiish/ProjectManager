using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectManager.Infrastructure.Configurations
{
    public class TaskEntityTypeConfiguration : IEntityTypeConfiguration<Task>
    {
        public void Configure(EntityTypeBuilder<Task> modelBuilder)
        {
            modelBuilder
               .HasMany(t => t.Assignees)
               .WithMany(u => u.Tasks);
            modelBuilder
               .HasOne(t => t.Status)
               .WithMany(s => s.Tasks);
            modelBuilder
               .HasMany(t => t.SubTasks)
               .WithOne(s => s.Task);
            modelBuilder
               .HasOne(t => t.CreatedBy)
               .WithMany(u => u.CreatedTasks)
               .HasForeignKey(t => t.CreatedById)
               .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
