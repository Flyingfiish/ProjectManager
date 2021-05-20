using ProjectManager.Application.DTOs.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.DTOs.User
{
    public class UserTasksDto
    {
        public List<TaskPreviewDto> Tasks { get; set; } = new List<TaskPreviewDto>();
    }
}
