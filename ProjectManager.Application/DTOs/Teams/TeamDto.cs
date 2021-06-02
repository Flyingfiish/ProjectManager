using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.DTOs.Teams
{
    public class TeamDto
    {
        public Guid Id { get; set; }

        public List<TeamParticipationDto> TeamUsers { get; set; } = new List<TeamParticipationDto>();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
