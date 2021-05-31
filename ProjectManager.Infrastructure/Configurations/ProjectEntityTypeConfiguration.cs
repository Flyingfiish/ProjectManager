using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Infrastructure.Configurations
{
    public class ProjectEntityTypeConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> modelBuilder)
        {
            modelBuilder
                .HasMany(p => p.Members)
                .WithMany(u => u.AssignedProjects)
                .UsingEntity<ProjectUser>(
                p => p
                .HasOne(p => p.User)
                .WithMany(p => p.ProjectUsers)
                .HasForeignKey(p => p.UserId),
                p => p
                .HasOne(p => p.Project)
                .WithMany(p => p.ProjectUsers)
                .HasForeignKey(p => p.ProjectId),
                p =>
                {
                    p.HasKey(p => new
                    {
                        p.Id
                    });
                    p
                    .HasOne(p => p.Team)
                    .WithMany()
                    .HasForeignKey(p => p.TeamId)
                    .OnDelete(DeleteBehavior.NoAction);
                });

            modelBuilder
                .HasOne(p => p.Manager)
                .WithMany(u => u.ManagedProjects)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder
                .HasOne(p => p.CreatedBy)
                .WithMany(u => u.CreatedProjects)
                .OnDelete(DeleteBehavior.NoAction);
            //modelBuilder
            //    .HasMany(p => p.Events)
            //    .WithOne(e => e.Project)
            //    .OnDelete(DeleteBehavior.NoAction);
            modelBuilder
                .HasMany(p => p.Tasks)
                .WithOne(t => t.Project)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder
                .HasMany(p => p.Statuses)
                .WithOne(t => t.Project)
                .OnDelete(DeleteBehavior.Cascade);


        }
    }
}
