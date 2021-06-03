using ProjectManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Domain.Specifications.TaskParticipations
{
    public class GetTaskParticipationSpecification : Specification<TaskParticipation>
    {
        public GetTaskParticipationSpecification(Guid taskId, Guid userId)
        {
            Predicate = t => t.TaskId == taskId && t.UserId == userId;
        }
    }
}
