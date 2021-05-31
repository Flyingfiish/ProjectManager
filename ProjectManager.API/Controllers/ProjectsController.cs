using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.Application.DTOs.Project;
using ProjectManager.Application.Interfaces;
using ProjectManager.Domain.Entities;
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
        public async Task<IActionResult> CreateTask(Guid projectId)
        {
            var id = User.Claims.Where(c => c.Type == "Id").Select(c => c.Value).SingleOrDefault();
            if (String.IsNullOrEmpty(id))
            {
                return Unauthorized();
            }
            Guid actorId = new Guid(id);

            try
            {
                using (var reader = new StreamReader(Request.Body))
                {
                    var body = await reader.ReadToEndAsync();
                    Domain.Entities.Task task = JsonSerializer.Deserialize<Domain.Entities.Task>(body);
                    await _projectsService.CreateTask(projectId, task, actorId);
                    return Ok();
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message + " \nInner exception" + e.InnerException);
            }
        }

        [Authorize]
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> MoveTask(Guid projectId, Guid taskId, Guid statusId, int index)
        {
            var id = User.Claims.Where(c => c.Type == "Id").Select(c => c.Value).SingleOrDefault();
            if (String.IsNullOrEmpty(id))
            {
                return Unauthorized();
            }
            Guid actorId = new Guid(id);

            try
            {
                await _projectsService.MoveTask(projectId, taskId, statusId, index, actorId);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message + " \nInner exception" + e.InnerException);
            }
        }

        [Authorize]
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> CreateStatus(Guid projectId)
        {
            var id = User.Claims.Where(c => c.Type == "Id").Select(c => c.Value).SingleOrDefault();
            if (String.IsNullOrEmpty(id))
            {
                return Unauthorized();
            }
            Guid actorId = new Guid(id);

            try
            {
                using (var reader = new StreamReader(Request.Body))
                {
                    var body = await reader.ReadToEndAsync();
                    Status status = JsonSerializer.Deserialize<Status>(body);
                    await _projectsService.CreateStatus(projectId, status, actorId);
                    return Ok();
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message + " \nInner exception" + e.InnerException);
            }
        }

        [Authorize]
        [HttpDelete]
        [Route("[action]")]
        public async Task<IActionResult> DeleteTask(Guid projectId, Guid taskId)
        {
            var id = User.Claims.Where(c => c.Type == "Id").Select(c => c.Value).SingleOrDefault();
            if (String.IsNullOrEmpty(id))
            {
                return Unauthorized();
            }
            Guid actorId = new Guid(id);

            try
            {
                await _projectsService.DeleteTask(projectId, taskId, actorId);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message + " \nInner exception" + e.InnerException);
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
                var response = await _projectsService.GetProject(projectId, actorId);
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetProjectTasks(Guid projectId)
        {
            var id = User.Claims.Where(c => c.Type == "Id").Select(c => c.Value).SingleOrDefault();
            if (String.IsNullOrEmpty(id))
            {
                return Unauthorized();
            }
            Guid actorId = new Guid(id);

            try
            {
                var response = await _projectsService.GetProjectTasks(projectId, actorId);
                return Ok(response);
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
    }
}
