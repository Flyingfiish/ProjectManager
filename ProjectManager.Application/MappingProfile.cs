using AutoMapper;
using ProjectManager.Application.DTOs.Project;
using ProjectManager.Application.DTOs.ProjectParticipations;
using ProjectManager.Application.DTOs.Status;
using ProjectManager.Application.DTOs.Task;
using ProjectManager.Application.DTOs.User;
using ProjectManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ProjectParticipation, ProjectProjectUserDto>();
            CreateMap<Project, ProjectPreviewDto>();
            CreateMap<Project, ProjectDto>();
            CreateMap<ProjectForCreateDto, Project>();
            CreateMap<ProjectParticipation, ProjectParticipationDto>();
            CreateMap<ProjectParticipation, ProjectParticipationWithoutUserDto>();

            CreateMap<Status, StatusDto>();

            CreateMap<Domain.Entities.Task, TaskPreviewDto>();

            CreateMap<User, UserShortDto>();
            CreateMap<User, UserBIODto>();
            CreateMap<RegisterRequest, User>()
                .ForMember(u => u.HashPassword, opt => opt.MapFrom(r => PasswordHasher.GetHash(r.Password)));
        }
    }
}
