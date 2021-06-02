using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.Application.Interfaces;
using ProjectManager.Domain.Entities;
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
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetStatuses(Guid projectId)
        {
            var id = User.Claims.Where(c => c.Type == "Id").Select(c => c.Value).SingleOrDefault();
            if (String.IsNullOrEmpty(id))
            {
                return Unauthorized();
            }
            Guid actorId = new Guid(id);

            try
            {
                var response = await _statusesService.GetStatuses(new GetStatusByProjectIdSpecification(projectId), new GetProjectParticipationByKeySpec(projectId, actorId));
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
