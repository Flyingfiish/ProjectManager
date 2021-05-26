using ProjectManager.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Domain.Entities
{
    public class Task : IGuidKey
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<User> Assignees { get; set; } = new List<User>();

        public Guid StatusId { get; set; }
        public Status Status { get; set; }
        public int Index { get; set; }

        public List<SubTask> SubTasks { get; set; } = new List<SubTask>();
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }

        public Guid? CreatedById { get; set; }
        public User CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public Guid? ProjectId { get; set; }
        public Project Project { get; set; }
    }
}
