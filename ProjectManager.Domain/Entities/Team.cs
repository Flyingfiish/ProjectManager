using ProjectManager.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Domain.Entities
{
    public class Team : IGuidKey
    {
        public Guid Id { get; set; }

        public List<TeamParticipation> Participations { get; set; } = new List<TeamParticipation>();
        public List<User> Members { get; set; } = new List<User>();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
