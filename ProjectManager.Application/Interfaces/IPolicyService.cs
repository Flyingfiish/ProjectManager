using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.Interfaces
{
    public interface IPolicyService
    {
        Task<bool> IsAllowedToTaskCRUD(Specification<ProjectParticipation> spec);
        Task<bool> IsAllowedToTaskMooving(Specification<ProjectParticipation> spece);
        Task<bool> IsAllowedToUserManagement(Specification<ProjectParticipation> spec);
        Task<bool> IsAllowedStatusCRUD(Specification<ProjectParticipation> spec);
        Task<bool> IsAllowedPMManagement(Specification<ProjectParticipation> spec);

        Task<bool> IsAllowedGetProject(Specification<ProjectParticipation> spec);
    }
}
