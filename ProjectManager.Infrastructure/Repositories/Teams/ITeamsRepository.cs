using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Infrastructure.Repositories.Teams
{
    public interface ITeamsRepository
    {
        public void UpdateTeamLeader(Guid teamId, Guid teamLeaderId);
        public void AddMember(Guid teamId, Guid memberId);
        public void DeleteMember(Guid teamId, Guid memberId);
    }
}
