using DataAccess.UnitOfWorks.Wfm;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Wfm.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IWfmUnitOfWork _unitofWork;
        public UsersController(IWfmUnitOfWork unitOfWork)
        {
            _unitofWork = unitOfWork;
        }

        [HttpGet]
        [Route("agents")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _unitofWork.UserRepository.FindAsync(u => u.Role == "Agent");
            return Ok(users);
        }
    }
}
