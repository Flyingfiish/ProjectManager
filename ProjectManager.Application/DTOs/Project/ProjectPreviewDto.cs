using ProjectManager.Application.DTOs.User;
using ProjectManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.DTOs.Project
{
    public class ProjectPreviewDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public List<UserShortDto> Members { get; set; } = new List<UserShortDto>();

        public string Description { get; set; }
        public string HexColor { get; set; }
        public ProjectState State { get; set; }
        public ProjectType ProjectType { get; set; }
        public DateTime CreatedAt { get; set; }

        public UserShortDto CreatedBy { get; set; }
    }
}
