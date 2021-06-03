using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.Interfaces
{
    public interface ITasksService
    {
        System.Threading.Tasks.Task Create(Domain.Entities.Task task, Guid actorId);
        System.Threading.Tasks.Task Delete(Specification<Domain.Entities.Task> spec, Guid actorId);

        System.Threading.Tasks.Task AddAssignee(Specification<Domain.Entities.Task> spec, Guid assigneeId, Guid actorId);
        System.Threading.Tasks.Task DeleteAssignee(Specification<TaskParticipation> spec, Guid actorId);

        System.Threading.Tasks.Task Update(Specification<Domain.Entities.Task> spec, Action<Domain.Entities.Task> func, Guid actorId);
        System.Threading.Tasks.Task Move(Specification<Domain.Entities.Task> spec, int index, Guid statusId, Guid actorId);

        Task<List<Domain.Entities.Task>> GetTasks(Specification<Domain.Entities.Task> spec, Guid actorId);

    }
}
