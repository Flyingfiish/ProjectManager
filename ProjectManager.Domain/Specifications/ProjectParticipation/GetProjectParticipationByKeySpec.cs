using ProjectManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Domain.Specifications.ProjectParticipations
{
    public class GetProjectParticipationByKeySpec : Specification<ProjectParticipation>
    {
        public GetProjectParticipationByKeySpec(Guid projectId, Guid userId)
        {
            Predicate = p => p.ProjectId == projectId && p.UserId == userId;
        }
    }
}
