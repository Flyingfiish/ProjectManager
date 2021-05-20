using ProjectManager.Application.DTOs.Task;
using ProjectManager.Application.DTOs.User;
using ProjectManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.DTOs.Project
{
    public class ProjectDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public List<ProjectProjectUserDto> ProjectUsers { get; set; } = new List<ProjectProjectUserDto>();

        public string Description { get; set; }
        public string HexColor { get; set; }
        public ProjectState State { get; set; }
        public ProjectType ProjectType { get; set; }
        public DateTime CreatedAt { get; set; }

        public UserShortDto CreatedBy { get; set; }

        public List<TaskPreviewDto> Tasks { get; set; } = new List<TaskPreviewDto>();
    }
}
