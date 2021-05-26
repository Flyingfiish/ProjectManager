using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectManager.Application.Interfaces
{
    public interface ITasksService
    {
        IAsyncEnumerable<Task> GetTasks(Specification<Task> spec);
    }
}
