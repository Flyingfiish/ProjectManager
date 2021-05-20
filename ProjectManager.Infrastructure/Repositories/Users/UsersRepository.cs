
using Microsoft.EntityFrameworkCore;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Specifications;
using ProjectManager.Domain.Specifications.Users;
using ProjectManager.Infrastructure.EFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Infrastructure.Repositories.Users
{
    public class UsersRepository : Repository<User>, IUsersRepository
    {
        public UsersRepository(ApplicationContext context) : base(context)
        {

        }
        //public void UpdateAvatarURL(Guid id, string avatarURL = null)
        //{
        //    User user = ReadOne(new GetUserByIdSpecification(id));
        //    user.HexColor = avatarURL;
        //    _context.SaveChanges();
        //}

        //public void UpdateBIO(UserBIODto userForUpdateBIODto)
        //{
        //    User user = ReadOne(new GetUserByIdSpecification(userForUpdateBIODto.Id));
        //    DtoMapper<UserBIODto, User>.SetProperties(userForUpdateBIODto, user);
        //    _context.SaveChanges();
        //}

        //public void UpdateLogin(Guid id, string login)
        //{
        //    User user = ReadOne(new GetUserByIdSpecification(id));
        //    user.Login = login;
        //    _context.SaveChanges();
        //}

        //public void UpdatePassword(Guid id, string newPassword)
        //{
        //    User user = ReadOne(new GetUserByIdSpecification(id));
        //    user.HashPassword = newPassword;
        //    _context.SaveChanges();
        //}
        

        public User ReadUserWithProjects(Specification<User> spec)
        {
            User user = _entities.Include(u => u.AssignedProjects).FirstOrDefault(spec.ToExpression());
            _context.Entry(user).State = EntityState.Detached;
            return user;
        }

        public async Task<IEnumerable<Project>> ReadUserWithProjectsAsync(Specification<User> spec)
        {
            User user = await _entities
                .Include(u => u.AssignedProjects).ThenInclude(p => p.CreatedBy)
                .Include(u => u.AssignedProjects).ThenInclude(p => p.Members)
                .FirstOrDefaultAsync(spec.ToExpression());
            _context.Entry(user).State = EntityState.Detached;
            return user.AssignedProjects;
        }

        public User ReadUserWithTasks(Specification<User> spec)
        {
            User user = _entities
                .Include(u => u.Tasks).ThenInclude(t => t.Assignees)
                .FirstOrDefault(spec.ToExpression());
            _context.Entry(user).State = EntityState.Detached;
            return user;
        }

        public async Task<IEnumerable<Domain.Entities.Task>> ReadUserWithTasksAsync(Specification<User> spec)
        {
            User user = await _entities.Include(u => u.Tasks).FirstOrDefaultAsync(spec.ToExpression());
            _context.Entry(user).State = EntityState.Detached;
            return user.Tasks;
        }
    }
}
