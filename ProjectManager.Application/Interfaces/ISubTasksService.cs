using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.Interfaces
{
    public interface ISubTasksService
    {
        System.Threading.Tasks.Task Create(SubTask subTask, Guid actorId);
        System.Threading.Tasks.Task Delete(Specification<SubTask> spec, Guid actorId);

        System.Threading.Tasks.Task Update(Specification<SubTask> spec, Action<SubTask> func, Guid actorId);
    }
}
