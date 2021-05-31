using ProjectManager.Application.Interfaces;
using ProjectManager.Domain.Specifications;
using ProjectManager.Infrastructure.Repositories.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.Services
{
    public class TasksService : ITasksService
    {
        private readonly TasksRepository _tasksRepository;
        public TasksService(TasksRepository tasksRepository)
        {
            _tasksRepository = tasksRepository;
        }

        public IAsyncEnumerable<Domain.Entities.Task> GetTasks(Specification<Domain.Entities.Task> spec)
        {
            return _tasksRepository.ReadMany(spec);
        }
    }
}
