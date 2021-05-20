using ProjectManager.Application.DTOs.Project;
using ProjectManager.Application.DTOs.Task;
using ProjectManager.Application.Interfaces;
using ProjectManager.Domain.Entities;
using ProjectManager.Infrastructure.Repositories.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.Services
{
    public class ProjectsService : IProjectsService
    {
        private readonly ProjectsRepository _projectsRepository;

        public ProjectsService(ProjectsRepository projectsRepository)
        {
            _projectsRepository = projectsRepository;
        }

        public void AddMember(Guid projectId, Guid memberId, ParticipationType participationType, Guid actorId)
        {
            throw new NotImplementedException();
        }

        public void AddTeam(Guid projectId, Guid teamId, Guid actorId)
        {
            throw new NotImplementedException();
        }

        public void Create(Project project)
        {
            throw new NotImplementedException();
        }

        public void CreateTask(Guid projectId, Domain.Entities.Task task, Guid actorId)
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid projectId, Guid actorId)
        {
            throw new NotImplementedException();
        }

        public void DeleteMember(Guid projectId, Guid memberId, Guid actorId)
        {
            throw new NotImplementedException();
        }

        public void DeleteTask(Guid projectId, Guid taskId, Guid actorId)
        {
            throw new NotImplementedException();
        }

        public void DeleteTeam(Guid projectId, Guid teamId, Guid actorId)
        {
            throw new NotImplementedException();
        }

        public ProjectDto GetProject(Guid projectId, Guid actorId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TaskPreviewDto> GetProjectTasks(Guid projectId, Guid actorId)
        {
            throw new NotImplementedException();
        }

        public void UpdateManager(Guid projectId, Guid managerId, Guid actorId)
        {
            throw new NotImplementedException();
        }
    }
}
