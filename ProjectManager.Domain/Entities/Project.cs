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
        public string Name { get; set; }

        public Guid? ManagerId { get; set; }
        public User Manager { get; set; }

        public List<User> Members { get; set; } = new List<User>();
        public List<ProjectUser> ProjectUsers { get; set; } = new List<ProjectUser>();

        public List<Task> Tasks { get; set; } = new List<Task>();

        public string Description { get; set; }
        public string HexColor { get; set; }
        public ProjectState State { get; set; }
        public ProjectType ProjectType { get; set; }
        public DateTime CreatedAt { get; set; }

        public Guid? CreatedById { get; set; }
        public User CreatedBy { get; set; }

        public List<Event> Events { get; set; } = new List<Event>();

        public List<Status> Statuses { get; set; } = new List<Status>();
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
}
