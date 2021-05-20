
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Infrastructure.Repositories.Users
{
    public interface IUsersRepository
    {
        public User ReadUserWithProjects(Specification<User> spec);
        public User ReadUserWithTasks(Specification<User> spec);
        //public void UpdatePassword(Guid id, string newPassword);
        //public void UpdateBIO(UserBIODto userForUpdateBIODto);
        //public void UpdateLogin(Guid id, string login);
        //public void UpdateAvatarURL(Guid id, string avatarURL);
    }
}
