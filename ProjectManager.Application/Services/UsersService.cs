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

        public UsersService(UsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        public UserShortDto GetUser(Specification<User> spec)
        {
            User user = _usersRepository.ReadOneAsync(spec).Result;
            if (user == null)
                return null;

            return new UserShortDto()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                SurName = user.SurName,
                LastName = user.LastName,
                HexColor = user.HexColor,
                Login = user.Login
            };
        }

        public UserBIODto GetUserBIO(Specification<User> spec)
        {
            User user = _usersRepository.ReadOneAsync(spec).Result;
            if (user == null)
                return null;
            return new UserBIODto()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                SurName = user.SurName,
                LastName = user.LastName,
                HexColor = user.HexColor,
                Login = user.Login,
                BirthDate = user.BirthDate,
                Sex = user.Sex
            };
        }

        public IEnumerable<ProjectPreviewDto> GetUserProjects(Specification<User> spec)
        {
            IEnumerable<Project> projects = _usersRepository.ReadUserWithProjectsAsync(spec).Result;
            var result = new List<ProjectPreviewDto>();
            foreach (var project in projects)
            {
                var projectPreview = new ProjectPreviewDto()
                {
                    CreatedAt = project.CreatedAt,
                    CreatedBy = new UserShortDto()
                    {
                        Id = project.CreatedBy.Id,
                        FirstName = project.CreatedBy.FirstName,
                        SurName = project.CreatedBy.SurName,
                        LastName = project.CreatedBy.LastName,
                        HexColor = project.CreatedBy.HexColor,
                        Login = project.CreatedBy.Login
                    },
                    Description = project.Description,
                    HexColor = project.HexColor,
                    Id = project.Id,
                    Name = project.Name,
                    ProjectType = project.ProjectType,
                    State = project.State,
                    Members = new List<UserShortDto>()
                };
                foreach (var member in project.Members)
                {
                    projectPreview.Members.Add(new UserShortDto()
                    {
                        Id = member.Id,
                        FirstName = member.FirstName,
                        SurName = member.SurName,
                        LastName = member.LastName,
                        HexColor = member.HexColor,
                        Login = member.Login
                    });
                }
                result.Add(projectPreview);
            }
            return result;
        }

        public IEnumerable<TaskPreviewDto> GetUserTasks(Specification<User> spec)
        {
            IEnumerable<Domain.Entities.Task> tasks = _usersRepository.ReadUserWithTasksAsync(spec).Result;
            var result = new List<TaskPreviewDto>();
            foreach (var task in tasks)
            {
                var taskPreview = new TaskPreviewDto()
                {
                    CreatedAt = task.CreatedAt,
                    Description = task.Description,
                    EndTime = task.EndTime,
                    Id = task.Id,
                    Name = task.Name,
                    ProjectId = task.ProjectId,
                    StartTime = task.StartTime,
                    Assignees = new List<UserShortDto>()
                };
                foreach (var assignee in task.Assignees)
                {
                    taskPreview.Assignees.Add(new UserShortDto()
                    {
                        Id = assignee.Id,
                        FirstName = assignee.FirstName,
                        SurName = assignee.SurName,
                        LastName = assignee.LastName,
                        HexColor = assignee.HexColor,
                        Login = assignee.Login
                    });
                }
                result.Add(taskPreview);
            }
            return result;
        }

        public void Register(RegisterRequest registerDto, CancellationToken cancellationToken = default)
        {
            _usersRepository.CreateAsync(new User()
            {
                Login = registerDto.Login,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                SurName = registerDto.SurName,
                HashPassword = PasswordHasher.GetHash(registerDto.Password)
            }).GetAwaiter();
        }

        public void UpdatePassword(Guid userId, string oldPassword, string newPassword, string newPasswordConfirmation)
        {
            User user = _usersRepository.ReadOneAsync(new GetUserByIdSpecification(userId)).Result;

            if(user.HashPassword != PasswordHasher.GetHash(oldPassword))
                throw new ArgumentException();

            if(newPassword != newPasswordConfirmation)
                throw new ArgumentException();

            _usersRepository.UpdateAsync(userId, u => u.HashPassword = PasswordHasher.GetHash(newPassword)).GetAwaiter();
        }

        public void UpdateUserBIO(UserBIODto userBIODto)
        {
            throw new NotImplementedException();
        }
    }
}
