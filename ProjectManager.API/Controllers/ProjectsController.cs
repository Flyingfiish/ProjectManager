using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Application.DTOs.Project;
using ProjectManager.Application.Interfaces;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Specifications.Common;
using ProjectManager.Domain.Specifications.ProjectParticipations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProjectManager.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectsService _projectsService;

        public ProjectsController(IProjectsService projectsService)
        {
            _projectsService = projectsService;
        }

        [Authorize]
        [HttpPut]
        [Route("[action]")]
        public async Task<IActionResult> Create()
        {
            var id = User.Claims.Where(c => c.Type == "Id").Select(c => c.Value).SingleOrDefault();
            if (String.IsNullOrEmpty(id))
            {
                return Unauthorized();
            }
            Guid createdById = new(id);

            using (var reader = new StreamReader(Request.Body))
            {
                var body = await reader.ReadToEndAsync();

                try
                {
                    ProjectForCreateDto projectForCreate = JsonSerializer.Deserialize<ProjectForCreateDto>(body);

                    await _projectsService.Create(projectForCreate, createdById);

                    return Ok();
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }
        }

        [Authorize]
        [HttpDelete]
        [Route("[action]")]
        public async Task<IActionResult> Delete(Guid projectId)
        {
            var id = User.Claims.Where(c => c.Type == "Id").Select(c => c.Value).SingleOrDefault();
            if (String.IsNullOrEmpty(id))
            {
                return Unauthorized();
            }
            Guid actorId = new Guid(id);

            try
            {
                await _projectsService.Delete(projectId, actorId);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> AddMember(Guid projectId, Guid memberId, ParticipationType participationType)
        {
            var id = User.Claims.Where(c => c.Type == "Id").Select(c => c.Value).SingleOrDefault();
            if (String.IsNullOrEmpty(id))
            {
                return Unauthorized();
            }
            Guid actorId = new(id);

            try
            {
                await _projectsService.AddMember(projectId, memberId, participationType, actorId);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpDelete]
        [Route("[action]")]
        public async Task<IActionResult> DeleteMember(Guid projectId, Guid memberId, ParticipationType participationType)
        {
            var id = User.Claims.Where(c => c.Type == "Id").Select(c => c.Value).SingleOrDefault();
            if (String.IsNullOrEmpty(id))
            {
                return Unauthorized();
            }
            Guid actorId = new(id);

            try
            {
                await _projectsService.DeleteMember(projectId, memberId, actorId);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> AddTeam(Guid projectId, Guid teamId)
        {
            var id = User.Claims.Where(c => c.Type == "Id").Select(c => c.Value).SingleOrDefault();
            if (String.IsNullOrEmpty(id))
            {
                return Unauthorized();
            }
            Guid actorId = new Guid(id);

            try
            {
                await _projectsService.AddTeam(projectId, teamId, actorId);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpDelete]
        [Route("[action]")]
        public async Task<IActionResult> DeleteTeam(Guid projectId, Guid teamId)
        {
            var id = User.Claims.Where(c => c.Type == "Id").Select(c => c.Value).SingleOrDefault();
            if (String.IsNullOrEmpty(id))
            {
                return Unauthorized();
            }
            Guid actorId = new Guid(id);

            try
            {
                await _projectsService.DeleteTeam(projectId, teamId, actorId);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }



        [Authorize]
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> UpdateManager(Guid projectId, Guid managerId)
        {
            var id = User.Claims.Where(c => c.Type == "Id").Select(c => c.Value).SingleOrDefault();
            if (String.IsNullOrEmpty(id))
            {
                return Unauthorized();
            }
            Guid actorId = new Guid(id);

            try
            {
                await _projectsService.UpdateManager(projectId, managerId, actorId);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetProject(Guid projectId)
        {
            var id = User.Claims.Where(c => c.Type == "Id").Select(c => c.Value).SingleOrDefault();
            if (String.IsNullOrEmpty(id))
            {
                return Unauthorized();
            }
            Guid actorId = new Guid(id);

            try
            {
                var response = await _projectsService.GetProject(
                    new GetByIdSpecification<Project>(projectId)
                    {
                        Includes = p => p
                        .Include(p => p.Participations)
                            .ThenInclude(p => p.User)
                        .Include(p => p.CreatedBy)
                        .Include(p => p.Statuses.OrderBy(s => s.Index))
                            .ThenInclude(s => s.Tasks.OrderBy(t => t.Index))
                    },
                    new GetProjectParticipationByKeySpec(projectId, actorId));
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
