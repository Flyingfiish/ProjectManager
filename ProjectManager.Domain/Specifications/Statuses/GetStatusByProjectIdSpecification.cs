using ProjectManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Domain.Specifications.Statuses
{
    public class GetStatusByProjectIdSpecification : Specification<Status>
    {
        public GetStatusByProjectIdSpecification(Guid projectId)
        {
            Predicate = p => p.ProjectId == projectId;
        }
    }
}
