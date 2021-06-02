using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Application.DTOs.Project;
using ProjectManager.Application.DTOs.Status;
using ProjectManager.Application.DTOs.Task;
using ProjectManager.Application.Interfaces;
using ProjectManager.Domain.Common;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Specifications;
using ProjectManager.Domain.Specifications.Common;
using ProjectManager.Domain.Specifications.ProjectParticipations;
using ProjectManager.Domain.Specifications.Projects;
using ProjectManager.Infrastructure.Repositories;
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
        private readonly IRepository<Project> _projectsRepository;
        private readonly IPolicyService _policyService;
        private readonly IRepository<Status> _statusesRepository;
        private readonly IRepository<ProjectParticipation> _projectParticipationRepository;
        private readonly IRepository<Team> _teamsRepository;
        private readonly IMapper _mapper;

        public ProjectsService(
            IRepository<Project> projectsRepository,
            IPolicyService policyService,
            IRepository<Status> statusesRepository,
            IRepository<ProjectParticipation> projectParticipationRepository,
            IRepository<Team> teamsRepository,
            IMapper mapper)
        {
            _projectsRepository = projectsRepository;
            _policyService = policyService;
            _statusesRepository = statusesRepository;
            _projectParticipationRepository = projectParticipationRepository;
            _teamsRepository = teamsRepository;
            _mapper = mapper;
        }

        public async System.Threading.Tasks.Task Create(ProjectForCreateDto projectForCreate, Guid actorId)
        {
            projectForCreate.Project.CreatedById = actorId;
            projectForCreate.Project.CreatedAt = DateTime.UtcNow;

            await _projectsRepository.Create(projectForCreate.Project);

            await _statusesRepository.Create(new Status() { ProjectId = projectForCreate.Project.Id, Name = "ToDo", Index = 0 });
            await _statusesRepository.Create(new Status() { ProjectId = projectForCreate.Project.Id, Name = "InProgress", Index = 1 });
            await _statusesRepository.Create(new Status() { ProjectId = projectForCreate.Project.Id, Name = "Done", Index = 2 });

            await _projectParticipationRepository.Create(new ProjectParticipation()
            {
                ProjectId = projectForCreate.Project.Id,
                UserId = actorId,
                TeamId = null,
                ParticipationType = ParticipationType.Creator
            });

            if (projectForCreate.Members != null)
                foreach (var memberId in projectForCreate.Members)
                {
                    await _projectParticipationRepository.Create(new ProjectParticipation()
                    {
                        ProjectId = projectForCreate.Project.Id,
                        UserId = memberId,
                        TeamId = null,
                        ParticipationType = ParticipationType.Executor
                    });
                }
        }

        public async System.Threading.Tasks.Task Delete(Guid projectId, Guid actorId)
        {
            if (await _policyService.IsAllowedPMManagement(new GetProjectParticipationByKeySpec(projectId, actorId)))
                await _projectsRepository.Delete(new GetByIdSpecification<Project>(projectId));
        }




        public async System.Threading.Tasks.Task AddMember(Guid projectId, Guid memberId, ParticipationType participationType, Guid actorId)
        {
            if (await _policyService.IsAllowedToUserManagement(new GetProjectParticipationByKeySpec(projectId, actorId)))
                await _projectParticipationRepository.Create(new ProjectParticipation()
                {
                    ProjectId = projectId,
                    UserId = memberId,
                    TeamId = null,
                    ParticipationType = participationType
                });
        }

        public async System.Threading.Tasks.Task DeleteMember(Guid projectId, Guid memberId, Guid actorId)
        {
            if (await _policyService.IsAllowedToUserManagement(new GetProjectParticipationByKeySpec(projectId, actorId)))
                await _projectParticipationRepository.Delete(new GetProjectParticipationByKeySpec(projectId, memberId));
        }

        public async System.Threading.Tasks.Task AddTeam(Guid projectId, Guid teamId, Guid actorId)
        {
            if (await _policyService.IsAllowedToUserManagement(new GetProjectParticipationByKeySpec(projectId, actorId)))
            {
                Team team = await _teamsRepository.ReadOne(
                    new GetByIdSpecification<Team>(teamId)
                    {
                        Includes = t => t.Include(t => t.Participations)
                    });

                foreach (var teamParticipation in team.Participations)
                {
                    await _projectParticipationRepository.Create(new ProjectParticipation()
                    {
                        ProjectId = projectId,
                        UserId = teamParticipation.UserId,
                        TeamId = teamParticipation.TeamId,
                        ParticipationType = teamParticipation.ParticipationType
                    });
                }
            }
        }

        public async System.Threading.Tasks.Task DeleteTeam(Guid projectId, Guid teamId, Guid actorId)
        {
            if (await _policyService.IsAllowedToUserManagement(new GetProjectParticipationByKeySpec(projectId, actorId)))
            {
                await _projectParticipationRepository.Delete(new GetProjectParticipationByTeamAndProjectSpec(teamId, projectId));
            }
        }






        public async Task<ProjectDto> GetProject(Specification<Project> projectSpec, Specification<ProjectParticipation> actorSpec)
        {
            bool isAllowed = await _policyService.IsAllowedGetProject(actorSpec);
            if (isAllowed)
            {
                var project = await _projectsRepository.ReadOne(projectSpec);
                var projectDto = _mapper.Map<ProjectDto>(project);
                return projectDto;
            }
            return null;
        }

        //public async Task<List<StatusDto>> GetStatuses(Guid projectId, Guid actorId)
        //{
        //    List<Status> statuses = await _projectsRepository.ReadStatuses(new GetProjectByIdSpecification(projectId));
        //    List<StatusDto> statusesDto = new List<StatusDto>();
        //    foreach (var status in statuses.OrderBy(s => s.Index))
        //    {
        //        var statusDto = new StatusDto()
        //        {
        //            Id = status.Id,
        //            Name = status.Name,
        //            Tasks = new List<TaskPreviewDto>()
        //        };
        //        foreach (var task in status.Tasks.OrderBy(t => t.Index))
        //        {
        //            var taskPreviewDto = new TaskPreviewDto()
        //            {
        //                Id = task.Id,
        //                Name = task.Name,
        //                StartTime = task.StartTime,
        //                EndTime = task.EndTime,
        //                CreatedAt = task.CreatedAt,
        //                ProjectId = task.ProjectId,
        //                StatusId = task.StatusId,
        //                Assignees = new List<DTOs.User.UserShortDto>()
        //            };
        //            foreach (var assignee in task.Assignees)
        //            {
        //                taskPreviewDto.Assignees.Add(new DTOs.User.UserShortDto()
        //                {
        //                    Id = assignee.Id,
        //                    Login = assignee.Login,
        //                    FirstName = assignee.FirstName,
        //                    LastName = assignee.LastName,
        //                    SurName = assignee.SurName,
        //                    HexColor = assignee.HexColor,
        //                });
        //            }
        //            statusDto.Tasks.Add(taskPreviewDto);
        //        }
        //    }
        //    return statusesDto;
        //}

        public async System.Threading.Tasks.Task UpdateManager(Guid projectId, Guid managerId, Guid actorId)
        {

            if (await _policyService.IsAllowedPMManagement(new GetProjectParticipationByKeySpec(projectId, actorId)))
            {
                await _projectParticipationRepository.Update(
                    new GetProjectParticipationByParticipationSpec(projectId, ParticipationType.ProjectManager),
                    p => p.ParticipationType = ParticipationType.Expert);

                await _projectParticipationRepository.Update(
                    new GetProjectParticipationByKeySpec(projectId, managerId),
                    p => p.ParticipationType = ParticipationType.ProjectManager);
            }
        }

        public async System.Threading.Tasks.Task Update(Specification<Project> spec, Action<Project> func, Specification<ProjectParticipation> actorSpec)
        {
            if (await _policyService.IsAllowedPMManagement(actorSpec))
            {
                await _projectsRepository.Update(spec, func);
            }
        }
    }
}
