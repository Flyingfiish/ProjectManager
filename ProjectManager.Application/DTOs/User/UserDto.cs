using ProjectManager.Application.DTOs.Project;
using ProjectManager.Application.DTOs.Task;
using ProjectManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.DTOs.User
{
    public class UserDto
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

        public List<ProjectPreviewDto> AssignedProjects { get; set; } = new List<ProjectPreviewDto>();

        public List<ProjectPreviewDto> CreatedProjects { get; set; } = new List<ProjectPreviewDto>();
        public List<ProjectPreviewDto> ManagedProjects { get; set; } = new List<ProjectPreviewDto>();

        public List<TaskPreviewDto> Tasks { get; set; } = new List<TaskPreviewDto>();
        public List<TaskPreviewDto> CreatedTasks { get; set; } = new List<TaskPreviewDto>();

        public List<Team> Teams { get; set; } = new List<Team>();
    }
}
