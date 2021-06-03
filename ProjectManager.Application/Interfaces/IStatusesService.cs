using ProjectManager.Application.DTOs.Status;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.Interfaces
{
    public interface IStatusesService
    {
        System.Threading.Tasks.Task Create(Status status, Guid actorId);
        System.Threading.Tasks.Task Delete(Guid projectId, Guid statusId, Guid actorId);

        Task<List<StatusDto>> GetStatuses(Specification<Status> spec, Specification<ProjectParticipation> actorSpec);

        System.Threading.Tasks.Task Update(Specification<Status> spec, Action<Status> func, Specification<ProjectParticipation> actorSpec);

        System.Threading.Tasks.Task Move(Specification<Status> spec, int index, Specification<ProjectParticipation> actorSpec);
    }
}
