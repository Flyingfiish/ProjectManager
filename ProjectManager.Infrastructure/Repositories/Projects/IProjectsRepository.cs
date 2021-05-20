using ProjectManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectManager.Infrastructure.Repositories.Projects
{
    public interface IProjectsRepository
    {
        //public void UpdateInfo(ProjectForUpdateInfoDto projectForUpdateInfoDto);
        public void UpdateManager(Guid projectId, Guid managerId);
        public void AddMember(Guid projectId, Guid membersIds, ParticipationType participationType);
        public void DeleteMember(Guid projectId, Guid membersIds);
        public void CreateTask(Guid projectId, Task task);
        public void AddTeam(Guid projectId, Guid teamId);
        public void DeleteTeam(Guid projectId, Guid teamId);
    }
}
