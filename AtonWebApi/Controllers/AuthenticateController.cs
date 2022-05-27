using AtonWebApi.Models;
using AtonWebApi.Response;
using AtonWebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace AtonWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {  
        private readonly AuthService _authService;
        public AuthenticateController(AuthService authService)
        {        
            _authService = authService;
        }
        [HttpPost("authenticate")]
        public async Task<ActionResult<string>> Authenticate([FromBody] LoginModel model)
        {
            var response =await _authService.AuthenticateAsync(model.Login, model.Password);
            if (response.StatusCode != AtonWebApi.Response.StatusCode.Ok)
            {
                return StatusCode((int)response.StatusCode,response.Description);
            }
            return Ok(new { token = ((BaseResponse<string>)response).Data });

        }
    }
}
