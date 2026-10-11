using Microsoft.AspNetCore.Mvc;
using DataAccess.DBContexts.Wfm.Models;
using DataAccess.UnitOfWorks.Wfm;

namespace Wfm.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffingController : ControllerBase
    {
        private readonly IWfmUnitOfWork _unitOfWork;

        public StaffingController(IWfmUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetStaffingRequirements()
        {
            var requirements = await _unitOfWork.StaffingRequirementsRepository.GetAllAsync();
            return Ok(requirements);
        }

        [HttpPost]
        public async Task<IActionResult> UpsertStaffingRequirement([FromBody] StaffingRequirement dto)
        {
            var existing = await _unitOfWork.StaffingRequirementsRepository.FirstOrDefaultAsync(s =>
                s.Date == dto.Date && s.TimePeriod == dto.TimePeriod
            );

            if (existing != null)
            {
                existing.RequiredAgents = dto.RequiredAgents;
                _unitOfWork.StaffingRequirementsRepository.Update(existing);
            }
            else
            {
                await _unitOfWork.StaffingRequirementsRepository.AddAsync(dto);
            }

            await _unitOfWork.SaveChangesAsync();
            return Ok(dto);
        }
    }
}