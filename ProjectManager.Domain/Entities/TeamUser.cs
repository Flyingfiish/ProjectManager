using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Domain.Entities
{
    public class TeamUser
    {
        public Guid TeamId { get; set; }
        public Team Team { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public ParticipationType ParticipationType { get; set; }
    }
}
