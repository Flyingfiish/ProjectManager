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
        public IActionResult Create()
        {
            var id = User.Claims.Where(c => c.Type == "Id").Select(c => c.Value).SingleOrDefault();
            if (String.IsNullOrEmpty(id))
            {
                return Unauthorized();
            }
            Guid createdById = new(id);

            using (var reader = new StreamReader(Request.Body))
            {
                var body = reader.ReadToEnd();

                try
                {
                    ProjectForCreateDto projectForCreate = JsonSerializer.Deserialize<ProjectForCreateDto>(body);
                    projectForCreate.Project.CreatedById = createdById;
                    projectForCreate.Project.CreatedAt = DateTime.UtcNow;
                    projectForCreate.Project.ManagerId = createdById;
                    _projectsService.Create(projectForCreate.Project);
                    foreach(var member in projectForCreate.Members)
                    {
                        _projectsService.AddMember(projectForCreate.Project.Id, member, ParticipationType.Executor, createdById);
                    }

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
        public IActionResult AddMember(Guid projectId, Guid memberId, ParticipationType participationType)
        {
            var id = User.Claims.Where(c => c.Type == "Id").Select(c => c.Value).SingleOrDefault();
            if (String.IsNullOrEmpty(id))
            {
                return Unauthorized();
            }
            Guid actorId = new(id);

            try
            {
                _projectsService.AddMember(projectId, memberId, participationType, actorId);
                return Ok();
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpDelete]
        [Route("[action]")]
        public IActionResult DeleteMember(Guid projectId, Guid memberId, ParticipationType participationType)
        {
            var id = User.Claims.Where(c => c.Type == "Id").Select(c => c.Value).SingleOrDefault();
            if (String.IsNullOrEmpty(id))
            {
                return Unauthorized();
            }
            Guid actorId = new(id);

            try
            {
                _projectsService.DeleteMember(projectId, memberId, actorId);
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
        public IActionResult CreateTask(Guid projectId)
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
                    var body = reader.ReadToEnd();
                    Domain.Entities.Task task = JsonSerializer.Deserialize<Domain.Entities.Task>(body);
                    _projectsService.CreateTask(projectId, task, actorId);
                    return Ok();
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpDelete]
        [Route("[action]")]
        public IActionResult DeleteTask(Guid projectId, Guid taskId)
        {
            var id = User.Claims.Where(c => c.Type == "Id").Select(c => c.Value).SingleOrDefault();
            if (String.IsNullOrEmpty(id))
            {
                return Unauthorized();
            }
            Guid actorId = new Guid(id);

            try
            {
                _projectsService.DeleteTask(projectId, taskId, actorId);
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
        public IActionResult UpdateManager(Guid projectId, Guid managerId)
        {
            var id = User.Claims.Where(c => c.Type == "Id").Select(c => c.Value).SingleOrDefault();
            if (String.IsNullOrEmpty(id))
            {
                return Unauthorized();
            }
            Guid actorId = new Guid(id);

            try
            {
                _projectsService.UpdateManager(projectId, managerId, actorId);
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
        public IActionResult GetProject(Guid projectId)
        {
            var id = User.Claims.Where(c => c.Type == "Id").Select(c => c.Value).SingleOrDefault();
            if (String.IsNullOrEmpty(id))
            {
                return Unauthorized();
            }
            Guid actorId = new Guid(id);

            try
            {
                var response = _projectsService.GetProject(projectId, actorId);
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
        public IActionResult GetProjectTasks(Guid projectId)
        {
            var id = User.Claims.Where(c => c.Type == "Id").Select(c => c.Value).SingleOrDefault();
            if (String.IsNullOrEmpty(id))
            {
                return Unauthorized();
            }
            Guid actorId = new Guid(id);

            try
            {
                var response = _projectsService.GetProjectTasks(projectId, actorId);
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
        public IActionResult AddTeam(Guid projectId, Guid teamId)
        {
            var id = User.Claims.Where(c => c.Type == "Id").Select(c => c.Value).SingleOrDefault();
            if (String.IsNullOrEmpty(id))
            {
                return Unauthorized();
            }
            Guid actorId = new Guid(id);

            try
            {
                _projectsService.AddTeam(projectId, teamId, actorId);
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
        public IActionResult DeleteTeam(Guid projectId, Guid teamId)
        {
            var id = User.Claims.Where(c => c.Type == "Id").Select(c => c.Value).SingleOrDefault();
            if (String.IsNullOrEmpty(id))
            {
                return Unauthorized();
            }
            Guid actorId = new Guid(id);

            try
            {
                _projectsService.DeleteTeam(projectId, teamId, actorId);
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
        public IActionResult Delete(Guid projectId)
        {
            var id = User.Claims.Where(c => c.Type == "Id").Select(c => c.Value).SingleOrDefault();
            if (String.IsNullOrEmpty(id))
            {
                return Unauthorized();
            }
            Guid actorId = new Guid(id);

            try
            {
                _projectsService.Delete(projectId, actorId);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
