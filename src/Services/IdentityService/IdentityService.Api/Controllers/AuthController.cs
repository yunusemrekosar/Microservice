using IdentityService.Api.Models;
using IdentityService.Api.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IIdentityService _identityService;

        public AuthController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequestModel requestModel)
        {
            var response = await _identityService.Login(requestModel);
            return Ok(response);
        }
    }
}
