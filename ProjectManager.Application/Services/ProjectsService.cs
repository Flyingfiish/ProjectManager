using AutoMapper;
using ProjectManager.Application.DTOs.Project;
using ProjectManager.Application.DTOs.Status;
using ProjectManager.Application.DTOs.Task;
using ProjectManager.Application.Interfaces;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Specifications.Projects;
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
        private readonly IMapper _mapper;

        public ProjectsService(ProjectsRepository projectsRepository, IMapper mapper)
        {
            _projectsRepository = projectsRepository;
            _mapper = mapper;
        }

        public async System.Threading.Tasks.Task AddMember(Guid projectId, Guid memberId, ParticipationType participationType, Guid actorId)
        {
            ParticipationType actorParticipationType = await _projectsRepository.ReadParticipationType(actorId, projectId);
            if (actorParticipationType == ParticipationType.Creator || actorParticipationType == ParticipationType.ProjectManager)
                await _projectsRepository.AddMember(projectId, memberId, participationType);
            else
                throw new Exception("You have no rights");
        }

        public async System.Threading.Tasks.Task DeleteMember(Guid projectId, Guid memberId, Guid actorId)
        {
            ParticipationType actorParticipationType = await _projectsRepository.ReadParticipationType(actorId, projectId);
            if (actorParticipationType == ParticipationType.Creator || actorParticipationType == ParticipationType.ProjectManager)
                await _projectsRepository.DeleteMember(projectId, memberId);
            else
                throw new Exception("You have no rights");
        }

        public async System.Threading.Tasks.Task AddTeam(Guid projectId, Guid teamId, Guid actorId)
        {
            ParticipationType actorParticipationType = await _projectsRepository.ReadParticipationType(actorId, projectId);
            if (actorParticipationType == ParticipationType.Creator || actorParticipationType == ParticipationType.ProjectManager)
                await _projectsRepository.AddTeam(projectId, teamId);
            else
                throw new Exception("You have no rights");
        }

        public async System.Threading.Tasks.Task DeleteTeam(Guid projectId, Guid teamId, Guid actorId)
        {
            ParticipationType actorParticipationType = await _projectsRepository.ReadParticipationType(actorId, projectId);
            if (actorParticipationType == ParticipationType.Creator || actorParticipationType == ParticipationType.ProjectManager)
                await _projectsRepository.DeleteTeam(projectId, teamId);
            else
                throw new Exception("You have no rights");
        }

        public async System.Threading.Tasks.Task Create(ProjectForCreateDto projectForCreate, Guid actorId)
        {
            projectForCreate.Project.CreatedById = actorId;
            projectForCreate.Project.CreatedAt = DateTime.UtcNow;
            projectForCreate.Project.ManagerId = actorId;

            await _projectsRepository.Create(projectForCreate.Project);
            await _projectsRepository.AddMember(projectForCreate.Project.Id, actorId, ParticipationType.Creator);

            if (projectForCreate.Members != null)
                foreach (var member in projectForCreate.Members)
                {
                    await AddMember(projectForCreate.Project.Id, member, ParticipationType.Executor, actorId);
                }
        }

        public async System.Threading.Tasks.Task CreateTask(Guid projectId, Domain.Entities.Task task, Guid actorId)
        {
            ParticipationType actorParticipationType = await _projectsRepository.ReadParticipationType(actorId, projectId);
            if (actorParticipationType == ParticipationType.Creator ||
                actorParticipationType == ParticipationType.ProjectManager ||
                actorParticipationType == ParticipationType.Expert ||
                actorParticipationType == ParticipationType.TeamLeader)
            {
                task.CreatedById = actorId;
                await _projectsRepository.CreateTask(projectId, task);
            }
            else
                throw new Exception("You have no rights");
        }

        public async System.Threading.Tasks.Task DeleteTask(Guid projectId, Guid taskId, Guid actorId)
        {
            ParticipationType actorParticipationType = await _projectsRepository.ReadParticipationType(actorId, projectId);
            if (actorParticipationType == ParticipationType.Creator ||
                actorParticipationType == ParticipationType.ProjectManager ||
                actorParticipationType == ParticipationType.Expert ||
                actorParticipationType == ParticipationType.TeamLeader)
                await _projectsRepository.DeleteTask(projectId, taskId);
            else
                throw new Exception("You have no rights");
        }

        public async System.Threading.Tasks.Task MoveTask(Guid projectId, Guid taskId, Guid statusId, int index, Guid actorId)
        {
            ParticipationType actorParticipationType = await _projectsRepository.ReadParticipationType(actorId, projectId);
            if (actorParticipationType == ParticipationType.Creator ||
                actorParticipationType == ParticipationType.ProjectManager ||
                actorParticipationType == ParticipationType.Expert ||
                actorParticipationType == ParticipationType.TeamLeader ||
                actorParticipationType == ParticipationType.Executor)
                await _projectsRepository.MoveTask(projectId, taskId, statusId, index);
            else
                throw new Exception("You have no rights");
        }

        public async System.Threading.Tasks.Task CreateStatus(Guid projectId, Status status, Guid actorId)
        {
            ParticipationType actorParticipationType = await _projectsRepository.ReadParticipationType(actorId, projectId);
            if (actorParticipationType == ParticipationType.Creator ||
                actorParticipationType == ParticipationType.ProjectManager)
                await _projectsRepository.CreateStatus(projectId, status);
            else
                throw new Exception("You have no rights");
        }

        public async System.Threading.Tasks.Task Delete(Guid projectId, Guid actorId)
        {
            ParticipationType actorParticipationType = await _projectsRepository.ReadParticipationType(actorId, projectId);
            if (actorParticipationType == ParticipationType.Creator)
                await _projectsRepository.Delete(projectId);
            else
                throw new Exception("You have no rights");
        }

        public async Task<ProjectDto> GetProject(Guid projectId, Guid actorId)
        {
            bool isAllowed = await _projectsRepository.IsAllowedToGet(projectId, actorId);
            if (isAllowed)
            {
                var project = await _projectsRepository.ReadOneIncludeAll(new GetProjectByIdSpecification(projectId));
                var projectDto = _mapper.Map<ProjectDto>(project);
                return projectDto;
            }
            else
                throw new Exception("You have no rights");
        }

        public async Task<IEnumerable<TaskPreviewDto>> GetProjectTasks(Guid projectId, Guid actorId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<StatusDto>> GetStatuses(Guid projectId, Guid actorId)
        {
            List<Status> statuses = await _projectsRepository.ReadStatuses(new GetProjectByIdSpecification(projectId));
            List<StatusDto> statusesDto = new List<StatusDto>();
            foreach (var status in statuses.OrderBy(s => s.Index))
            {
                var statusDto = new StatusDto()
                {
                    Id = status.Id,
                    Name = status.Name,
                    Tasks = new List<TaskPreviewDto>()
                };
                foreach (var task in status.Tasks.OrderBy(t => t.Index))
                {
                    var taskPreviewDto = new TaskPreviewDto()
                    {
                        Id = task.Id,
                        Name = task.Name,
                        StartTime = task.StartTime,
                        EndTime = task.EndTime,
                        CreatedAt = task.CreatedAt,
                        ProjectId = task.ProjectId,
                        StatusId = task.StatusId,
                        Assignees = new List<DTOs.User.UserShortDto>()
                    };
                    foreach (var assignee in task.Assignees)
                    {
                        taskPreviewDto.Assignees.Add(new DTOs.User.UserShortDto()
                        {
                            Id = assignee.Id,
                            Login = assignee.Login,
                            FirstName = assignee.FirstName,
                            LastName = assignee.LastName,
                            SurName = assignee.SurName,
                            HexColor = assignee.HexColor,
                        });
                    }
                    statusDto.Tasks.Add(taskPreviewDto);
                }
            }
            return statusesDto;
        }

        public async System.Threading.Tasks.Task UpdateManager(Guid projectId, Guid managerId, Guid actorId)
        {
            ParticipationType actorParticipationType = await _projectsRepository.ReadParticipationType(actorId, projectId);
            if (actorParticipationType == ParticipationType.Creator || actorParticipationType == ParticipationType.ProjectManager)
                await _projectsRepository.UpdateManager(projectId, managerId);
            else
                throw new Exception("You have no rights");
        }
    }
}
