using AutoMapper;
using ProjectManager.Application.DTOs.Project;
using ProjectManager.Application.DTOs.Task;
using ProjectManager.Application.DTOs.User;
using ProjectManager.Application.Interfaces;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Specifications;
using ProjectManager.Domain.Specifications.Users;
using ProjectManager.Infrastructure.EFCore;
using ProjectManager.Infrastructure.Repositories;
using ProjectManager.Infrastructure.Repositories.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectManager.Application.Services
{
    public class UsersService : IUsersService
    {
        private readonly UsersRepository _usersRepository;
        private readonly IMapper _mapper;

        public UsersService(UsersRepository usersRepository, IMapper mapper)
        {
            _usersRepository = usersRepository;
            _mapper = mapper;
        }

        public async Task<UserShortDto> GetUser(Specification<User> spec)
        {
            User user = await _usersRepository.ReadOneAsync(spec);
            if (user == null)
                return null;
            return _mapper.Map<UserShortDto>(user);
        }

        public async Task<UserBIODto> GetUserBIO(Specification<User> spec)
        {
            User user = await _usersRepository.ReadOneAsync(spec);
            if (user == null)
                return null;
            return _mapper.Map<UserBIODto>(user);
        }

        public async Task<IEnumerable<ProjectPreviewDto>> GetUserProjects(Specification<User> spec)
        {
            IEnumerable<Project> projects = await _usersRepository.ReadUserWithProjectsAsync(spec);
            var result = _mapper.Map<List<ProjectPreviewDto>>(projects);
            return result;
        }

        public async Task<IEnumerable<TaskPreviewDto>> GetUserTasks(Specification<User> spec)
        {
            IEnumerable<Domain.Entities.Task> tasks = await _usersRepository.ReadUserWithTasksAsync(spec);
            var result = _mapper.Map<List<TaskPreviewDto>>(tasks);
            return result;
        }

        public async System.Threading.Tasks.Task Register(RegisterRequest registerDto, CancellationToken cancellationToken = default)
        {
            await _usersRepository.CreateAsync(_mapper.Map<User>(registerDto));
        }

        public async System.Threading.Tasks.Task UpdatePassword(Guid userId, string oldPassword, string newPassword, string newPasswordConfirmation)
        {
            User user = await _usersRepository.ReadOneAsync(new GetUserByIdSpecification(userId));

            if (user.HashPassword != PasswordHasher.GetHash(oldPassword))
                throw new ArgumentException();

            if (newPassword != newPasswordConfirmation)
                throw new ArgumentException();

            _usersRepository.UpdateAsync(userId, u => u.HashPassword = PasswordHasher.GetHash(newPassword)).GetAwaiter();
        }

        public async System.Threading.Tasks.Task UpdateUserBIO(UserBIODto userBIODto)
        {
            throw new NotImplementedException();
        }
    }
}
