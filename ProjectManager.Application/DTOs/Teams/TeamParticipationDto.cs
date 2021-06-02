using ProjectManager.Application.DTOs.User;
using ProjectManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.DTOs.Teams
{
    public class TeamParticipationDto
    {
        public UserShortDto User { get; set; }

        public ParticipationType ParticipationType { get; set; }
    }
}
