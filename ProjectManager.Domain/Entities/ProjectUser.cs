using ProjectManager.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Domain.Entities
{
    public class ProjectUser : IGuidKey
    {
        public Guid Id { get; set; }

        public Guid ProjectId { get; set; }
        public Project Project { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public Guid? TeamId { get; set; }
        public Team Team { get; set; }

        public ParticipationType ParticipationType { get; set; }
    }

    public enum ParticipationType
    {
        Creator,
        Expert,
        TeamLeader,
        ProjectManager,
        Executor
    }
}
