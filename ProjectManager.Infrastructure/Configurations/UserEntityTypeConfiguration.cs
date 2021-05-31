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
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> modelBuilder)
        {
            modelBuilder
                .HasIndex(u => u.Login)
                .IsUnique();

            //modelBuilder
            //    .HasMany(u => u.Actions)
            //    .WithOne(e => e.Actor);

            modelBuilder
                .HasMany(u => u.Teams)
                .WithMany(t => t.Members)
                .UsingEntity<TeamUser>(
                t => t
                .HasOne(t => t.Team)
                .WithMany(u => u.TeamUsers)
                .HasForeignKey(t => t.TeamId),
                t => t
                .HasOne(t => t.User)
                .WithMany(t => t.TeamUsers)
                .HasForeignKey(t => t.UserId),
                t =>
                {
                    t.HasKey(t => new
                    {
                        t.TeamId,
                        t.UserId
                    });
                });
        }
    }
}
