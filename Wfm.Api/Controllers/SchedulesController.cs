using Common.DataTransferObjects;
using DataAccess.DBContexts.Wfm.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Wfm.Api.Service.Interface;

namespace Wfm.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchedulesController : ControllerBase
    {
        private readonly IScheduleService _scheduleService;

        public SchedulesController(IScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<ScheduleDetails>> GetScheduleByName([FromRoute] int id)
        {
            var schedules = await _scheduleService.GetByScheduleId(id);

            return schedules;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Schedule>>> GetSchedules()
        {
            var schedules = await _scheduleService.GetAllSchedulesAsync();
            return Ok(schedules);
        }

        [HttpPost]
        public async Task<ActionResult<Schedule>> CreateSchedule([FromBody] ScheduleDetails dto)
        {
            var (success, message, schedule) = await _scheduleService.CreateScheduleAsync(dto);

            if (!success)
            {
                return BadRequest(new { message });
            }

            return CreatedAtAction(nameof(GetSchedules), new { id = schedule?.Id }, schedule);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSchedule(int id)
        {
            var deleted = await _scheduleService.DeleteScheduleAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
