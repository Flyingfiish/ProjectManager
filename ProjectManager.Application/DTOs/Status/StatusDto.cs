using ProjectManager.Application.DTOs.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.DTOs.Status
{
    public class StatusDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<TaskPreviewDto> Tasks { get; set; } = new List<TaskPreviewDto>();
    }
}
