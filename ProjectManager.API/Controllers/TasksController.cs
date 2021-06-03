using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.Application.Interfaces;
using ProjectManager.Domain.Specifications.Common;
using ProjectManager.Domain.Specifications.TaskParticipations;
using ProjectManager.Domain.Specifications.Tasks;
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
    public class TasksController : Controller
    {
        private readonly ITasksService _tasksService;

        public TasksController(ITasksService tasksService)
        {
            _tasksService = tasksService;
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
            Guid actorId = new Guid(id);

            try
            {
                using (var reader = new StreamReader(Request.Body))
                {
                    var body = await reader.ReadToEndAsync();
                    Domain.Entities.Task task = JsonSerializer.Deserialize<Domain.Entities.Task>(body);
                    task.CreatedById = actorId;
                    await _tasksService.Create(task, actorId);
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
        public async Task<IActionResult> Delete(Guid taskId)
        {
            var id = User.Claims.Where(c => c.Type == "Id").Select(c => c.Value).SingleOrDefault();
            if (String.IsNullOrEmpty(id))
            {
                return Unauthorized();
            }
            Guid actorId = new Guid(id);

            try
            {
                await _tasksService.Delete(new GetByIdSpecification<Domain.Entities.Task>(taskId), actorId);
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
        public async Task<IActionResult> AddAssignee(Guid taskId, Guid assigneeId)
        {
            var id = User.Claims.Where(c => c.Type == "Id").Select(c => c.Value).SingleOrDefault();
            if (String.IsNullOrEmpty(id))
            {
                return Unauthorized();
            }
            Guid actorId = new Guid(id);

            try
            {
                await _tasksService.AddAssignee(new GetByIdSpecification<Domain.Entities.Task>(taskId), assigneeId, actorId);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message + " \nInner exception" + e.InnerException);
            }
        }

        [Authorize]
        [HttpDelete]
        [Route("[action]")]
        public async Task<IActionResult> DeleteAssignee(Guid taskId, Guid assigneeId)
        {
            var id = User.Claims.Where(c => c.Type == "Id").Select(c => c.Value).SingleOrDefault();
            if (String.IsNullOrEmpty(id))
            {
                return Unauthorized();
            }
            Guid actorId = new Guid(id);

            try
            {
                await _tasksService.DeleteAssignee(new GetTaskParticipationSpecification(taskId, assigneeId), actorId);
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
        public async Task<IActionResult> Move(Guid projectId, Guid taskId, Guid statusId, int index)
        {
            var id = User.Claims.Where(c => c.Type == "Id").Select(c => c.Value).SingleOrDefault();
            if (String.IsNullOrEmpty(id))
            {
                return Unauthorized();
            }
            Guid actorId = new Guid(id);

            try
            {
                await _tasksService.Move(new GetTaskByIdAndProjectIdSpecification(taskId, projectId), index, statusId, actorId);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message + " \nInner exception" + e.InnerException);
            }
        }
    }
}
