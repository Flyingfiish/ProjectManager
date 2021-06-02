using ProjectManager.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Domain.Entities
{
    public class SubTask : IGuidKey
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public SubTaskStatus Status { get; set; }

        public Guid TaskId { get; set; }
        public Task Task { get; set; }
    }

    public enum SubTaskStatus
    {
        InProcess,
        Done
    }
}
