using ProjectManager.Application.DTOs.Project;
using ProjectManager.Application.DTOs.Task;
using ProjectManager.Application.DTOs.User;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectManager.Application.Interfaces
{
    public interface IUsersService
    {
        System.Threading.Tasks.Task Register(RegisterRequest registerDto, CancellationToken cancellationToken = default);
        Task<UserShortDto> GetUser(Specification<User> spec);
        Task<UserBIODto> GetUserBIO(Specification<User> spec);
        Task<IEnumerable<ProjectPreviewDto>> GetUserProjects(Specification<User> spec);
        Task<IEnumerable<TaskPreviewDto>> GetUserTasks(Specification<User> spec);
        System.Threading.Tasks.Task UpdateUserBIO(UserBIODto userBIODto);
        System.Threading.Tasks.Task UpdatePassword(Guid userId, string oldPassword, string newPassword, string newPasswordConfirmation);
    }
}
