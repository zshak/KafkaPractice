using KafkaConsumer.Services.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KafkaConsumer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("[Action]")]
        public async Task<IActionResult> GetRange(int userId)
        {
            return Ok(await _userService.GetUserBetRange(userId));
        }

        [HttpGet("[Action]")]
        public async Task<IActionResult> GetLeaderboard()
        {
            return Ok(await _userService.GetLeaderBoard());
        }
    }
}
