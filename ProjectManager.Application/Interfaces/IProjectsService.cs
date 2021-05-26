using ProjectManager.Application.DTOs.Project;
using ProjectManager.Application.DTOs.Status;
using ProjectManager.Application.DTOs.Task;
using ProjectManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.Interfaces
{
    public interface IProjectsService
    {
        System.Threading.Tasks.Task Create(ProjectForCreateDto projectForCreate, Guid actorId);
        System.Threading.Tasks.Task AddMember(Guid projectId, Guid memberId, ParticipationType participationType, Guid actorId);
        System.Threading.Tasks.Task DeleteMember(Guid projectId, Guid memberId, Guid actorId);
        System.Threading.Tasks.Task CreateTask(Guid projectId, Domain.Entities.Task task, Guid actorId);
        System.Threading.Tasks.Task DeleteTask(Guid projectId, Guid taskId, Guid actorId);
        System.Threading.Tasks.Task UpdateManager(Guid projectId, Guid managerId, Guid actorId);
        Task<List<StatusDto>> GetStatuses(Guid projectId, Guid actorId);
        Task<ProjectDto> GetProject(Guid projectId, Guid actorId);
        Task<IEnumerable<TaskPreviewDto>> GetProjectTasks(Guid projectId, Guid actorId);
        System.Threading.Tasks.Task AddTeam(Guid projectId, Guid teamId, Guid actorId);
        System.Threading.Tasks.Task DeleteTeam(Guid projectId, Guid teamId, Guid actorId);
        System.Threading.Tasks.Task Delete(Guid projectId, Guid actorId);
    }
}
