using Microsoft.EntityFrameworkCore;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Specifications;
using ProjectManager.Domain.Specifications.Projects;
using ProjectManager.Domain.Specifications.Users;
using ProjectManager.Infrastructure.EFCore;
using ProjectManager.Infrastructure.Repositories.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectManager.Infrastructure.Repositories.Projects
{
    public class ProjectsRepository : Repository<Project>, IProjectsRepository
    {
        public ProjectsRepository(ApplicationContext context) : base(context)
        {

        }

        public void AddMember(Guid projectId, Guid memberId, ParticipationType participationType)
        {
            _context.ProjectUsers.Add(
                new ProjectUser()
                {
                    ProjectId = projectId,
                    TeamId = null,
                    UserId = memberId,
                    ParticipationType = participationType
                });
            _context.SaveChanges();
        }

        public void DeleteMember(Guid projectId, Guid memberId)
        {
            var participation = _context.ProjectUsers.FirstOrDefault(p => p.ProjectId == projectId && p.UserId == memberId);
            _context.ProjectUsers.Remove(participation);
            _context.SaveChanges();
        }

        public void CreateTask(Guid projectId, Task task)
        {
            Project project = ReadOne(new GetProjectByIdSpecification(projectId));
            project.Tasks.Add(task);
            Event e =
                new Event()
                {
                    Actor = task.CreatedBy,
                    EventType = EventType.Created,
                    CreatedAt = DateTime.Now,
                    Target = task
                };
            project.Events.Add(e);
            _context.SaveChanges();
        }

        public void DeleteTask(Guid projectId, Guid taskId)
        {
            Task task = _context.Tasks.FirstOrDefault(t => t.Id == taskId);
            _entities.FirstOrDefault(p => p.Id == projectId).Tasks.Remove(task);
            _context.SaveChanges();
        }

        public void UpdateManager(Guid projectId, Guid managerId)
        {
            Project project = ReadOne(new GetProjectByIdSpecification(projectId));

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

            _context.SaveChanges();
        }

        public override Project ReadOne(Specification<Project> spec)
        {
            return _entities
                .Include(p => p.ProjectUsers).ThenInclude(p => p.User)
                .Include(p => p.CreatedBy)
                .Include(p => p.Manager)
                .Include(p => p.Events)
                .Include(p => p.Statuses)
                .Include(p => p.Tasks)
                .FirstOrDefault(spec.ToExpression());
        }

        public void AddTeam(Guid projectId, Guid teamId)
        {
            var team = _context.Teams.Include(t => t.TeamUsers).FirstOrDefault(t => t.Id == teamId);
            //_context.ProjectUsers
            //    .Add(new ProjectUser()
            //    {
            //        ProjectId = projectId,
            //        TeamId = teamId,
            //        UserId = team.TeamLeaderId,
            //        ParticipationType = ProjectParticipationType.TeamLeader
            //    });

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

            _context.SaveChanges();
        }

        public void DeleteTeam(Guid projectId, Guid teamId)
        {
            var participations = _context.ProjectUsers.Where(p => p.TeamId == teamId);

            _context.ProjectUsers.RemoveRange(participations);

            _context.SaveChanges();
        }

        public void Delete(Guid projectId)
        {
            var project = _context.Projects.FirstOrDefault(p => p.Id == projectId);
            project.Statuses.Clear();
            _entities.Remove(project);
            _context.SaveChanges();
        }
    }
}
