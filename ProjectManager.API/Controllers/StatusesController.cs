using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.Application.DTOs.Status;
using ProjectManager.Application.Interfaces;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Specifications.Common;
using ProjectManager.Domain.Specifications.ProjectParticipations;
using ProjectManager.Domain.Specifications.Statuses;
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
    public class StatusesController : Controller
    {
        private readonly IStatusesService _statusesService;

        public StatusesController(IStatusesService statusesService)
        {
            _statusesService = statusesService;
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
                    Status status = JsonSerializer.Deserialize<Status>(body);
                    await _statusesService.Create(status, actorId);
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
        public async Task<IActionResult> Delete(Guid statusId, Guid projectId)
        {
            var id = User.Claims.Where(c => c.Type == "Id").Select(c => c.Value).SingleOrDefault();
            if (String.IsNullOrEmpty(id))
            {
                return Unauthorized();
            }
            Guid actorId = new Guid(id);

            try
            {
                await _statusesService.Delete(projectId, statusId, actorId);
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
        public async Task<List<StatusDto>> Get(Guid projectId)
        {
            var id = User.Claims.Where(c => c.Type == "Id").Select(c => c.Value).SingleOrDefault();
            Guid actorId = new Guid(id);

            var response = await _statusesService.GetStatuses(new GetStatusByProjectIdSpecification(projectId), new GetProjectParticipationByKeySpec(projectId, actorId));
            return response;
        }

        [Authorize]
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> UpdateName(Guid statusId, Guid projectId, string newName)
        {
            var id = User.Claims.Where(c => c.Type == "Id").Select(c => c.Value).SingleOrDefault();
            if (String.IsNullOrEmpty(id))
            {
                return Unauthorized();
            }
            Guid actorId = new Guid(id);

            try
            {
                await _statusesService.Update(new GetByIdSpecification<Status>(statusId), s => s.Name = newName, new GetProjectParticipationByKeySpec(projectId, actorId));
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message + "\n" + e.InnerException);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Move(Guid statusId, Guid projectId, int index)
        {
            var id = User.Claims.Where(c => c.Type == "Id").Select(c => c.Value).SingleOrDefault();
            if (String.IsNullOrEmpty(id))
            {
                return Unauthorized();
            }
            Guid actorId = new Guid(id);

            try
            {
                await _statusesService.Move(new GetByIdSpecification<Status>(statusId), index, new GetProjectParticipationByKeySpec(projectId, actorId));
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
