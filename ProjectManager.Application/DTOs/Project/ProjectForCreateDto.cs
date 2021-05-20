using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.DTOs.Project
{
    public class ProjectForCreateDto
    {
        public Domain.Entities.Project Project { get; set; }
        public IEnumerable<Guid> Members { get; set; }
    }
}
