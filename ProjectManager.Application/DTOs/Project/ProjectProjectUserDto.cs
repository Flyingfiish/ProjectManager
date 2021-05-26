using ProjectManager.Application.DTOs.User;
using ProjectManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.DTOs.Project
{
    public class ProjectProjectUserDto
    {
        public UserShortDto User { get; set; }

        public Guid? TeamId { get; set; }

        public ParticipationType ParticipationType { get; set; }
    }
}
