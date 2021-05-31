using ProjectManager.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Domain.Entities
{
    public class User : IGuidKey
    {
        public Guid Id { get; set; }
        public string Login { get; set; }
        public string HashPassword { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SurName { get; set; }
        public Sex Sex { get; set; }
        public DateTime? BirthDate { get; set; }
        public string HexColor { get; set; }

        public List<Project> AssignedProjects { get; set; } = new List<Project>();
        public List<ProjectUser> ProjectUsers { get; set; } = new List<ProjectUser>();

        public List<Project> CreatedProjects { get; set; } = new List<Project>();
        public List<Project> ManagedProjects { get; set; } = new List<Project>();

        public List<Task> Tasks { get; set; } = new List<Task>();
        public List<Task> CreatedTasks { get; set; } = new List<Task>();

        public List<Team> Teams { get; set; } = new List<Team>();
        public List<TeamUser> TeamUsers { get; set; } = new List<TeamUser>();

        //public List<Event> Actions { get; set; } = new List<Event>();
    }

    public enum Sex
    {
        Male,
        Female
    }
}
