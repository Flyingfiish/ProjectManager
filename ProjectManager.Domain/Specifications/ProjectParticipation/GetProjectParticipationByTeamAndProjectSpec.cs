using ProjectManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Domain.Specifications.ProjectParticipations
{
    public class GetProjectParticipationByTeamAndProjectSpec : Specification<ProjectParticipation>
    {
        public GetProjectParticipationByTeamAndProjectSpec(Guid teamId, Guid projectId)
        {
            Predicate = t => t.TeamId == teamId && t.ProjectId == projectId;
        }
    }
}
