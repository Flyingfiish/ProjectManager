using ProjectManager.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Domain.Entities
{
    public class Event : IGuidKey
    {
        public Guid Id { get; set; }
        public EventType EventType { get; set; }

        public Guid? ActorId { get; set; }
        public User Actor { get; set; }

        public Guid? TargetId { get; set; }
        public Task Target { get; set; }

        public Guid? ProjectId { get; set; }
        public Project Project { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public enum EventType
    {
        Created,
        Changed,
        Deleted
    }
}
