using ProjectManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Infrastructure.Repositories.Projects
{
    public interface IProjectsRepository
    {
        //public void UpdateInfo(ProjectForUpdateInfoDto projectForUpdateInfoDto);
        public System.Threading.Tasks.Task UpdateManager(Guid projectId, Guid managerId);
        public System.Threading.Tasks.Task AddMember(Guid projectId, Guid membersIds, ParticipationType participationType);
        public System.Threading.Tasks.Task DeleteMember(Guid projectId, Guid membersIds);
        public System.Threading.Tasks.Task CreateTask(Guid projectId, Domain.Entities.Task task);
        public System.Threading.Tasks.Task AddTeam(Guid projectId, Guid teamId);
        public System.Threading.Tasks.Task DeleteTeam(Guid projectId, Guid teamId);
        public Task<bool> IsAllowedToGet(Guid projectId, Guid userId);
    }
}
