using DataAccess.UnitOfWorks.Wfm;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
    }
}
