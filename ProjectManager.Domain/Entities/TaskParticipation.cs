using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Domain.Entities
{
    public class TaskParticipation
    {
        public Guid TaskId { get; set; }
        public Task Task { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
