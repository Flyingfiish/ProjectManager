using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Application.DTOs.Project;
using ProjectManager.Application.DTOs.ProjectParticipations;
using ProjectManager.Application.DTOs.Task;
using ProjectManager.Application.DTOs.User;
using ProjectManager.Application.Interfaces;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Specifications;
using ProjectManager.Domain.Specifications.Users;
using ProjectManager.Infrastructure.EFCore;
using ProjectManager.Infrastructure.Extentions;
using ProjectManager.Infrastructure.Repositories;
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
        private readonly IRepository<User> _usersRepository;
        private readonly IMapper _mapper;

        public UsersService(IRepository<User> usersRepository, IMapper mapper)
        {
            _usersRepository = usersRepository;
            _mapper = mapper;
        }

        public async Task<UserShortDto> GetShort(Specification<User> spec)
        {
            User user = await _usersRepository.ReadOne(spec);

            return _mapper.Map<UserShortDto>(user);
        }

        public async Task<UserBIODto> GetUserBIO(Specification<User> spec)
        {
            User user = await _usersRepository.ReadOne(spec);

            return _mapper.Map<UserBIODto>(user);
        }

        public async Task<IEnumerable<ProjectParticipationWithoutUserDto>> GetProjects(Specification<User> spec)
        {
            spec.Includes = u => u
            .Include(u => u.ProjectParticipations)
            .ThenInclude(p => p.Project)
            .ThenInclude(p => p.Members);

            User user = await _usersRepository.ReadOne(spec);

            IEnumerable<ProjectParticipation> projects = user.ProjectParticipations;
            var result = _mapper.Map<List<ProjectParticipationWithoutUserDto>>(projects);

            return result;
        }

        public async Task<IEnumerable<TaskPreviewDto>> GetTasks(Specification<User> spec)
        {
            spec.Includes = u => u.Include(u => u.Tasks);

            User user = await _usersRepository.ReadOne(spec);

            IEnumerable<Domain.Entities.Task> tasks = user.Tasks;
            var result = _mapper.Map<List<TaskPreviewDto>>(tasks);

            return result;
        }

        public async System.Threading.Tasks.Task Register(RegisterRequest registerDto, CancellationToken cancellationToken = default)
        {
            await _usersRepository.Create(_mapper.Map<User>(registerDto));
        }

        public async System.Threading.Tasks.Task Update(Specification<User> spec, Action<User> func, Guid actorId)
        {
            User user = await _usersRepository.ReadOne(spec);
            if (user.Id == actorId)
                throw new Exception("You do not have rights");
            await _usersRepository.Update(spec, func);
        }

        public async System.Threading.Tasks.Task UpdatePassword(Specification<User> spec, string oldPassword, string newPassword, string newPasswordConfirmation, Guid actorId)
        {
            User user = await _usersRepository.ReadOne(spec);

            if (user.HashPassword != PasswordHasher.GetHash(oldPassword))
                throw new ArgumentException();

            if (newPassword != newPasswordConfirmation)
                throw new ArgumentException();

            _usersRepository.Update(spec, u => u.HashPassword = PasswordHasher.GetHash(newPassword)).GetAwaiter();
        }
    }
}
