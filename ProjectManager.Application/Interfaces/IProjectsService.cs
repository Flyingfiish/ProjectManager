using ProjectManager.Application.DTOs.Project;
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
        void Create(Project project);
        void AddMember(Guid projectId, Guid memberId, ParticipationType participationType, Guid actorId);
        void DeleteMember(Guid projectId, Guid memberId, Guid actorId);
        void CreateTask(Guid projectId, Domain.Entities.Task task, Guid actorId);
        void DeleteTask(Guid projectId, Guid taskId, Guid actorId);
        void UpdateManager(Guid projectId, Guid managerId, Guid actorId);
        ProjectDto GetProject(Guid projectId, Guid actorId);
        IEnumerable<TaskPreviewDto> GetProjectTasks(Guid projectId, Guid actorId);
        void AddTeam(Guid projectId, Guid teamId, Guid actorId);
        void DeleteTeam(Guid projectId, Guid teamId, Guid actorId);
        void Delete(Guid projectId, Guid actorId);
    }
}
