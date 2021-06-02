using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Application.DTOs.Status;
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
    public class StatusesService : IStatusesService
    {
        private readonly IRepository<Status> _statusesRepository;
        private readonly IPolicyService _policyService;
        private readonly IMapper _mapper;


        public StatusesService(IRepository<Status> statusesRepository, IPolicyService policyService, IMapper maper)
        {
            _statusesRepository = statusesRepository;
            _policyService = policyService;
            _mapper = maper;
        }

        public async System.Threading.Tasks.Task Create(Status status, Guid actorId)
        {
            if(await _policyService.IsAllowedPMManagement(new GetProjectParticipationByKeySpec(status.ProjectId, actorId)))
            {
                await _statusesRepository.Create(status);
            }
        }

        public async System.Threading.Tasks.Task Delete(Guid projectId, Guid statusId, Guid actorId)
        {
            if (await _policyService.IsAllowedPMManagement(new GetProjectParticipationByKeySpec(projectId, actorId)))
            {
                await _statusesRepository.Delete(new GetByIdSpecification<Status>(statusId));
            }
        }

        public async Task<IAsyncEnumerable<StatusDto>> GetStatuses(Specification<Status> spec, Specification<ProjectParticipation> actorSpec)
        {
            if (await _policyService.IsAllowedGetProject(actorSpec))
            {
                return _mapper.Map<IAsyncEnumerable<StatusDto>>(_statusesRepository.ReadMany(spec));
            }
            return null;
        }

        public async System.Threading.Tasks.Task Move(Specification<Status> spec, int index, Specification<ProjectParticipation> actorSpec)
        {
            if (await _policyService.IsAllowedPMManagement(actorSpec))
            {
                spec.Includes = s => s.Include(s => s.Project).ThenInclude(p => p.Statuses);
                var status = await _statusesRepository.ReadOne(spec);
                var statuses = status.Project.Statuses.OrderBy(s => s.Index).ToList();

                statuses.RemoveAt(status.Index);
                statuses.Insert(index, status);

                for (int i = 0; i < statuses.Count; i++)
                {
                    await _statusesRepository.Update(new GetByIdSpecification<Status>(statuses[i].Id), t => t.Index = i);
                }
            }
        }

        public async System.Threading.Tasks.Task Update(Specification<Status> spec, Action<Status> func, Specification<ProjectParticipation> actorSpec)
        {
            if(await _policyService.IsAllowedPMManagement(actorSpec))
            {
                await _statusesRepository.Update(spec, func);
            }
        }
    }
}
