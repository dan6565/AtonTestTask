using AtonWebApi.Models;
using AtonWebApi.Response;
using AtonWebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AtonWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly AuthService _authService;
        public AuthenticateController( IConfiguration configuration, AuthService authService)
        {          
            _configuration = configuration;
            _authService = authService;
        }
        [HttpPost("authenticate")]
        public async Task<ActionResult<string>> Authenticate([FromBody] LoginModel model)
        {
            var response =await _authService.Authenticate(model.Login, model.Password);
            if (response.StatusCode != AtonWebApi.Response.StatusCode.Ok)
            {
                return StatusCode((int)response.StatusCode,response.Description);
            }
            return Ok(((BaseResponse<string>)response).Data);

        }
    }
}
