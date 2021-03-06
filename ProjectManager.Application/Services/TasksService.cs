using Microsoft.EntityFrameworkCore;
using ProjectManager.Application.Interfaces;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Specifications;
using ProjectManager.Domain.Specifications.Common;
using ProjectManager.Domain.Specifications.ProjectParticipations;
using ProjectManager.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.Services
{
    public class TasksService : ITasksService
    {
        private readonly IRepository<Domain.Entities.Task> _tasksRepository;
        private readonly IRepository<TaskParticipation> _partisipationsRepository;
        private readonly IRepository<Status> _statusesRepository;
        private readonly IPolicyService _policyService;

        public TasksService(
            IRepository<Domain.Entities.Task> tasksRepository,
            IRepository<TaskParticipation> partisipationsRepository,
            IRepository<Status> statusesRepository,
            IPolicyService policyService)
        {
            _tasksRepository = tasksRepository;
            _partisipationsRepository = partisipationsRepository;
            _statusesRepository = statusesRepository;
            _policyService = policyService;
        }

        public async Task<List<Domain.Entities.Task>> GetTasks(Specification<Domain.Entities.Task> spec, Guid actorId)
        {
            return await _tasksRepository.ReadMany(spec);
        }

        public async System.Threading.Tasks.Task Create(Domain.Entities.Task task, Guid actorId)
        {
            task.CreatedAt = DateTime.UtcNow;
            await _tasksRepository.Create(task);
        }

        public async System.Threading.Tasks.Task Delete(Specification<Domain.Entities.Task> spec, Guid actorId)
        {
            await _tasksRepository.Delete(spec);
        }

        public async System.Threading.Tasks.Task AddAssignee(Specification<Domain.Entities.Task> spec, Guid assigneeId, Guid actorId)
        {
            Domain.Entities.Task task = await _tasksRepository.ReadOne(spec);
            if (await _policyService.IsAllowedToTaskCRUD(new GetProjectParticipationByKeySpec(task.ProjectId, actorId)))
            {
                await _partisipationsRepository.Create(new TaskParticipation() { TaskId = task.Id, UserId = assigneeId });
                await _tasksRepository.Update(spec, t => t.UpdatedAt = DateTime.UtcNow);
            }
        }

        public async System.Threading.Tasks.Task DeleteAssignee(Specification<TaskParticipation> spec, Guid actorId)
        {
            await _partisipationsRepository.Delete(spec);

        }

        public async System.Threading.Tasks.Task Update(Specification<Domain.Entities.Task> spec, Action<Domain.Entities.Task> func, Guid actorId)
        {
            Domain.Entities.Task task = await _tasksRepository.ReadOne(spec);
            if (await _policyService.IsAllowedToTaskCRUD(new GetProjectParticipationByKeySpec(task.ProjectId, actorId)))
            {
                await _tasksRepository.Update(spec, func);
                await _tasksRepository.Update(spec, t => t.UpdatedAt = DateTime.UtcNow);
            }
        }

        public async System.Threading.Tasks.Task Move(Specification<Domain.Entities.Task> spec, int index, Guid statusId, Guid actorId)
        {
            spec.Includes = t => t.Include(t => t.Project).ThenInclude(p => p.Statuses).ThenInclude(s => s.Tasks);
            Domain.Entities.Task task = await _tasksRepository.ReadOne(spec);
            Project project = task.Project;

            if (task.StatusId == statusId)
            {
                var status = await _statusesRepository.ReadOne(new GetByIdSpecification<Status>(statusId) { Includes = s => s.Include(s => s.Tasks) });
                var tasks = status.Tasks.OrderBy(t => t.Index).ToList();

                tasks.RemoveAt(task.Index);
                tasks.Insert(index, task);

                for (int i = 0; i < tasks.Count; i++)
                {
                    await _tasksRepository.Update(new GetByIdSpecification<Domain.Entities.Task>(tasks[i].Id), t => t.Index = i);
                }
            }
            else
            {
                var newStatus = await _statusesRepository.ReadOne(new GetByIdSpecification<Status>(statusId));
                var oldStatus = await _statusesRepository.ReadOne(new GetByIdSpecification<Status>(task.StatusId));

                var newTasks = newStatus.Tasks.OrderBy(t => t.Index).ToList();
                var oldTasks = oldStatus.Tasks.OrderBy(t => t.Index).ToList();

                await _tasksRepository.Update(new GetByIdSpecification<Domain.Entities.Task>(task.Id), t => t.StatusId = statusId);

                oldTasks.RemoveAt(task.Index);
                newTasks.Insert(index, task);

                for (int i = 0; i < newTasks.Count; i++)
                {
                    await _tasksRepository.Update(new GetByIdSpecification<Domain.Entities.Task>(newTasks[i].Id), t => t.Index = i);
                }

                for (int i = 0; i < oldTasks.Count; i++)
                {
                    await _tasksRepository.Update(new GetByIdSpecification<Domain.Entities.Task>(oldTasks[i].Id), t => t.Index = i);
                }
            }
        }
    }
}
