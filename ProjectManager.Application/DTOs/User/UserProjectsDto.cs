using ProjectManager.Application.DTOs.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.DTOs.User
{
    public class UserProjectsDto
    {
        public List<ProjectPreviewDto> AssignedProjects { get; set; }
    }
}
