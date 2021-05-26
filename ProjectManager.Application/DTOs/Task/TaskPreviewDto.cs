using ProjectManager.Application.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.DTOs.Task
{
    public class TaskPreviewDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<UserShortDto> Assignees { get; set; } = new List<UserShortDto>();
        public Guid StatusId { get; set; }

        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public DateTime CreatedAt { get; set; }

        public Guid? ProjectId { get; set; }
    }
}
