using ProjectManager.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Domain.Entities
{
    public class Project : IGuidKey
    {
        public Guid Id { get; set; }
        public string Title { get; set; }

        public List<User> Members { get; set; } = new List<User>();
        public List<ProjectParticipation> Participations { get; set; } = new List<ProjectParticipation>();

        public List<Status> Statuses { get; set; } = new List<Status>();
        public List<Task> Tasks { get; set; } = new List<Task>();

        public string Description { get; set; }
        public string HexColor { get; set; }
        public ProjectState State { get; set; }
        public ProjectType Type { get; set; }
        public ProjectPrivacy Privacy { get; set; }
        public DateTime CreatedAt { get; set; }

        public Guid? CreatedById { get; set; }
        public User CreatedBy { get; set; }
    }

    public enum ProjectState
    {
        InProgress,
        Completed
    }

    public enum ProjectType
    {
        Kanban,
        Scrum
    }

    public enum ProjectPrivacy
    {
        Public,
        Private
    }
}
