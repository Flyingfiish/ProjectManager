using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Specifications;
using ProjectManager.Domain.Specifications.Common;
using ProjectManager.Domain.Specifications.Projects;
using ProjectManager.Domain.Specifications.Users;
using ProjectManager.Infrastructure.EFCore;
using ProjectManager.Infrastructure.Repositories.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Infrastructure.Repositories.Projects
{
    public class ProjectsRepository : Repository<Project>, IProjectsRepository
    {


        public ProjectsRepository(ApplicationContext context) : base(context)
        {
        }

        public async Task<ParticipationType> ReadParticipationType(Guid userId, Guid projectId)
        {
            ProjectUser projectUser = await _context.ProjectUsers.FirstOrDefaultAsync(p => p.UserId == userId);
            return projectUser.ParticipationType;
        }

        public async System.Threading.Tasks.Task AddMember(Guid projectId, Guid memberId, ParticipationType participationType)
        {
            _context.ProjectUsers.Add(
                new ProjectUser()
                {
                    ProjectId = projectId,
                    TeamId = null,
                    UserId = memberId,
                    ParticipationType = participationType
                });
            await _context.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task DeleteMember(Guid projectId, Guid memberId)
        {
            var participation = _context.ProjectUsers.FirstOrDefault(p => p.ProjectId == projectId && p.UserId == memberId);
            _context.ProjectUsers.Remove(participation);
            await _context.SaveChangesAsync();
        }

        public async Task<Project> ReadOneIncludeStatuses(Specification<Project> spec)
        {
            return await _entities
                .Include(p => p.Statuses).ThenInclude(s => s.Tasks)
                .FirstOrDefaultAsync(spec.ToExpression());
        }

        public async System.Threading.Tasks.Task CreateTask(Guid projectId, Domain.Entities.Task task)
        {

            Project project = await ReadOneIncludeStatuses(new GetByIdSpecification<Project>(projectId));
            task.Index = project.Statuses.FirstOrDefault(new GetByIdSpecification<Status>(task.StatusId)).Tasks.Count;
            project.Tasks.Add(task);
            await _context.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task MoveTask(Guid projectId, Guid taskId, Guid statusId, int index)
        {
            Project project = await ReadOneIncludeStatuses(new GetByIdSpecification<Project>(projectId));

            var status = project.Statuses.FirstOrDefault(s => s.Id == statusId);

            var movingTask = project.Tasks.FirstOrDefault(t => t.Id == taskId);

            if (movingTask.Index == index && movingTask.StatusId == statusId)
                return;

            movingTask.Index = index;
            movingTask.StatusId = statusId;

            foreach (var task in status.Tasks.Where(t => t.Index >= index && t.Id != taskId))
            {
                task.Index += 1;
            }

            await _context.SaveChangesAsync();

        }

        public async System.Threading.Tasks.Task DeleteTask(Guid projectId, Guid taskId)
        {
            Domain.Entities.Task task = _context.Tasks.FirstOrDefault(t => t.Id == taskId);
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task CreateStatus(Guid projectId, Status status)
        {
            Project project = await ReadOneIncludeStatuses(new GetProjectByIdSpecification(projectId));
            status.Index = project.Statuses.Count;
            project.Statuses.Add(status);
            await _context.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task UpdateManager(Guid projectId, Guid managerId)
        {
            Project project = await ReadOne(new GetProjectByIdSpecification(projectId));

            if (project.ManagerId != null)
                _context
                    .ProjectUsers
                    .FirstOrDefault(p => p.ProjectId == projectId && p.UserId == project.ManagerId)
                    .ParticipationType = ParticipationType.Expert;

            _context
                .ProjectUsers
                .FirstOrDefault(p => p.ProjectId == projectId && p.UserId == managerId)
                .ParticipationType = ParticipationType.ProjectManager;

            project.ManagerId = managerId;

            await _context.SaveChangesAsync();
        }

        public async Task<Project> ReadOneIncludeAll(Specification<Project> spec)
        {
            return await _entities
                .Include(p => p.ProjectUsers).ThenInclude(p => p.User)
                .Include(p => p.CreatedBy)
                .Include(p => p.Manager)
                //.Include(p => p.Events)
                .Include(p => p.Statuses)
                .Include(p => p.Tasks)
                .FirstOrDefaultAsync(spec.ToExpression());
        }

        public async Task<List<Status>> ReadStatuses(Specification<Project> spec)
        {
            var project = await _entities
                .Include(p => p.Statuses)
                .ThenInclude(s => s.Tasks.OrderBy(t => t.Index))
                .ThenInclude(t => t.Assignees)
                .FirstOrDefaultAsync(spec.ToExpression());
            return project.Statuses;
        }

        public async System.Threading.Tasks.Task AddTeam(Guid projectId, Guid teamId)
        {
            var team = _context.Teams.Include(t => t.TeamUsers).FirstOrDefault(t => t.Id == teamId);

            var participations = new List<ProjectUser>();
            foreach (var e in team.TeamUsers)
            {
                participations.Add(new ProjectUser()
                {
                    ProjectId = projectId,
                    TeamId = e.TeamId,
                    UserId = e.UserId,
                    ParticipationType = e.ParticipationType
                });
            }

            _context.ProjectUsers.AddRange(participations);

            await _context.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task DeleteTeam(Guid projectId, Guid teamId)
        {
            var participations = _context.ProjectUsers.Where(p => p.TeamId == teamId);

            _context.ProjectUsers.RemoveRange(participations);

            await _context.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task Delete(Guid projectId)
        {
            var project = _context.Projects.FirstOrDefault(p => p.Id == projectId);
            project.Statuses.Clear();
            _entities.Remove(project);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsAllowedToGet(Guid projectId, Guid userId)
        {
            var project = await _entities.FirstOrDefaultAsync(p => p.Id == projectId);
            var user = await _context.ProjectUsers.FirstOrDefaultAsync(p => p.UserId == userId && p.ProjectId == projectId);
            if (user != null)
                return true;
            else if (project.Privacy == ProjectPrivacy.Public)
                return true;
            return false;
        }
    }
}
