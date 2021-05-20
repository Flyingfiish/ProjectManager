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
        void Register(RegisterRequest registerDto, CancellationToken cancellationToken = default);
        UserShortDto GetUser(Specification<User> spec);
        UserBIODto GetUserBIO(Specification<User> spec);
        IEnumerable<ProjectPreviewDto> GetUserProjects(Specification<User> spec);
        IEnumerable<TaskPreviewDto> GetUserTasks(Specification<User> spec);
        void UpdateUserBIO(UserBIODto userBIODto);
        void UpdatePassword(Guid userId, string oldPassword, string newPassword, string newPasswordConfirmation);
    }
}
