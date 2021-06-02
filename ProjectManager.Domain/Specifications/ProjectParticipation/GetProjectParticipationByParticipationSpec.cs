using ProjectManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Domain.Specifications.ProjectParticipations
{
    public class GetProjectParticipationByParticipationSpec : Specification<ProjectParticipation>
    {
        public GetProjectParticipationByParticipationSpec(Guid projectId, ParticipationType participationType)
        {
            Predicate = p => p.ProjectId == projectId && p.ParticipationType == participationType;
        }
    }
}
