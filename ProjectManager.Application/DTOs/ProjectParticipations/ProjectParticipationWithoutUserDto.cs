using ProjectManager.Application.DTOs.Project;
using ProjectManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.DTOs.ProjectParticipations
{
    public class ProjectParticipationWithoutUserDto
    {
        public ProjectPreviewDto Project { get; set; }

        public Guid? TeamId { get; set; }

        public ParticipationType ParticipationType { get; set; }
    }
}
