using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Domain.Specifications.Tasks
{
    public class GetTaskByIdAndProjectIdSpecification : Specification<Domain.Entities.Task>
    {
        public GetTaskByIdAndProjectIdSpecification(Guid taskId, Guid projectId)
        {
            Predicate = t => t.Id == taskId && t.ProjectId == projectId;
        }
    }
}
