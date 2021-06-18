using Microsoft.EntityFrameworkCore;
using ProjectManager.Application.Interfaces;
using ProjectManager.Domain.Common;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Specifications;
using ProjectManager.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.Services
{
    public class PolicyService : IPolicyService
    {
        private readonly IRepository<ProjectParticipation> _projectsParticiparionRepository;

        public PolicyService(IRepository<ProjectParticipation> projectsParticiparionRepository, IRepository<Project> projectsRepository)
        {
            _projectsParticiparionRepository = projectsParticiparionRepository;
        }

        public async Task<ProjectParticipation> GetHighestParticipation(Specification<ProjectParticipation> spec)
        {
            List<ProjectParticipation> projectParticipations = await _projectsParticiparionRepository.ReadMany(spec);
            ProjectParticipation highestParticipation = new ProjectParticipation() { ParticipationType = ParticipationType.Executor};

            foreach (var projectParticipation in projectParticipations)
            {
                if ((int)highestParticipation.ParticipationType > (int)projectParticipation.ParticipationType)
                {
                    highestParticipation = projectParticipation;
                }
            }
            return highestParticipation;
        }

        public async Task<bool> IsAllowedGetProject(Specification<ProjectParticipation> spec)
        {
            spec.Includes = pp => pp.Include(pp => pp.Project);

            ProjectParticipation projectParticipation = await _projectsParticiparionRepository.ReadOne(spec);

            if (projectParticipation != null)
                return true;
            return false;
        }

        public async Task<bool> IsAllowedPMManagement(Specification<ProjectParticipation> spec)
        {
            ProjectParticipation projectParticipation = await GetHighestParticipation(spec);
            return ProjectActionPolicy.PMManagement(projectParticipation.ParticipationType);
        }

        public async Task<bool> IsAllowedStatusCRUD(Specification<ProjectParticipation> spec)
        {
            ProjectParticipation projectParticipation = await GetHighestParticipation(spec);
            return ProjectActionPolicy.StatusCRUD(projectParticipation.ParticipationType);
        }

        public async Task<bool> IsAllowedToTaskCRUD(Specification<ProjectParticipation> spec)
        {
            ProjectParticipation projectParticipation = await GetHighestParticipation(spec);
            return ProjectActionPolicy.TaskCRUD(projectParticipation.ParticipationType);
        }

        public async Task<bool> IsAllowedToTaskMooving(Specification<ProjectParticipation> spec)
        {
            ProjectParticipation projectParticipation = await GetHighestParticipation(spec);
            return ProjectActionPolicy.TaskMooving(projectParticipation.ParticipationType);
        }

        public async Task<bool> IsAllowedToUserManagement(Specification<ProjectParticipation> spec)
        {
            ProjectParticipation projectParticipation = await GetHighestParticipation(spec);
            return ProjectActionPolicy.UserManagement(projectParticipation.ParticipationType);
        }
    }
}
