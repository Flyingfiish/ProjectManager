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

        Task<UserShortDto> GetShortUser(Specification<User> spec);
        Task<UserBIODto> GetUserBIO(Specification<User> spec);
        Task<IEnumerable<ProjectPreviewDto>> GetUserProjects(Specification<User> spec);
        Task<IEnumerable<TaskPreviewDto>> GetUserTasks(Specification<User> spec);

        System.Threading.Tasks.Task Update(Specification<User> spec, Action<User> func, Guid actorId);
        //System.Threading.Tasks.Task UpdateUserBIO(UserBIODto userBIODto);
        System.Threading.Tasks.Task UpdatePassword(Specification<User> spec, string oldPassword, string newPassword, string newPasswordConfirmation, Guid actorId);
    }
}
