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
               .WithMany(u => u.Tasks)
               .UsingEntity<TaskParticipation>(
                tp => tp
                .HasOne(tp => tp.User)
                .WithMany(u => u.TaskParticipations)
                .HasForeignKey(tp => tp.UserId),
                tp => tp
                .HasOne(tp => tp.Task)
                .WithMany(t => t.TaskParticipations)
                .HasForeignKey(tp => tp.TaskId),
                tp =>
                {
                    tp.HasKey(tp => new
                    {
                        tp.TaskId,
                        tp.UserId
                    });
                }
                );
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
