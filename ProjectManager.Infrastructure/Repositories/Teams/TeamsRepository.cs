using Microsoft.EntityFrameworkCore;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Specifications;
using ProjectManager.Infrastructure.EFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Infrastructure.Repositories.Teams
{
    public class TeamsRepository : Repository<Team>, ITeamsRepository
    {
        public TeamsRepository(ApplicationContext context): base(context)
        {

        }

        public void UpdateTeamLeader(Guid teamId, Guid teamLeaderId)
        {
            TeamUser teamLeader = _context.TeamUsers
                .FirstOrDefault(t => t.TeamId == teamId && t.ParticipationType.Equals(ParticipationType.TeamLeader));
            if (teamLeader != null && teamLeader.UserId != teamLeaderId)
            {
                teamLeader.ParticipationType = ParticipationType.Executor;
                _context.Entry(teamLeader).State = EntityState.Detached;
            }
            
            TeamUser newTeamLeader = _context.TeamUsers.FirstOrDefault(t => t.UserId == teamLeaderId && t.TeamId == teamId);
            newTeamLeader.ParticipationType = ParticipationType.TeamLeader;
            _context.SaveChanges();
        }

        public override Team ReadOne(Specification<Team> spec)
        {
            return _entities
                .Include(t => t.TeamUsers).ThenInclude(t => t.User)
                .FirstOrDefault(spec.ToExpression());
        }

        public void AddMember(Guid teamId, Guid memberId)
        {
            TeamUser newMember = new TeamUser()
            {
                TeamId = teamId,
                UserId = memberId,
                ParticipationType = ParticipationType.Executor
            };
            _context.TeamUsers.Add(newMember);
            _context.SaveChanges();
        }

        public void DeleteMember(Guid teamId, Guid memberId)
        {
            var participation = _context.TeamUsers.FirstOrDefault(p => p.TeamId == teamId && p.UserId == memberId);
            _context.TeamUsers.Remove(participation);
            _context.SaveChanges();
        }
    }
}
