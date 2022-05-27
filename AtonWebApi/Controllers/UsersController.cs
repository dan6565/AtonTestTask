using AtonTestTask.Interfaces;
using AtonWebApi.Entities;
using AtonWebApi.Models;
using AtonWebApi.Response;
using AtonWebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace AtonWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UsersService _usersService;
        private readonly AuthService _authService;

        public UsersController(UsersService usersService, AuthService authService) 
        { 
            _usersService = usersService;
            _authService = authService;
        }
        [HttpPost("Create")]
        public async Task<ActionResult> CreateUserAsync([FromBody] ModelCreating model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = _authService.VerifyAdmin(model.Token);
            if (response.StatusCode != AtonWebApi.Response.StatusCode.Ok)
            {
                return StatusCode((int)response.StatusCode, response.Description);
            }
            var creator = _authService.GetUserLogin(model.Token);
            response = await _usersService.CreateUserAsync(creator,model.User);

            return StatusCode((int)response.StatusCode, response.Description);

        }
        [HttpPatch("UpdateLogin")]
        public async Task<ActionResult> UpdateLoginAsync([FromBody] ModelUpdateLogin model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = _authService.CheckAccessAdminOrPerformingUser(model.Token, model.PreviousUserLogin);
            if (response.StatusCode != AtonWebApi.Response.StatusCode.Ok)
            {
                return StatusCode((int)response.StatusCode, response.Description);
            }
            var modifiedBy = _authService.GetUserLogin(model.Token);
            response = await _usersService.UpdateLoginAsync(modifiedBy, model.PreviousUserLogin, model.NewUserLogin);
            
            return StatusCode((int)response.StatusCode, response.Description);
        }
        [HttpPatch("UpdatePassword")]
        public async Task<ActionResult> UpdatePasswordAsync([FromBody] ModelUpdatePassword model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
           
            var response = _authService.CheckAccessAdminOrPerformingUser(model.Token, model.UserLogin);
            if (response.StatusCode != AtonWebApi.Response.StatusCode.Ok)
            {
                return StatusCode((int)response.StatusCode, response.Description);
            }
            var modifiedBy = _authService.GetUserLogin(model.Token);
             response = await _usersService.UpdatePasswordAsync(modifiedBy, model.UserLogin, model.NewUserPassword);
            return StatusCode((int)response.StatusCode, response.Description);
        }
        [HttpPatch("UpdateName")]
        public async Task<ActionResult> UpdateNameAsync([FromBody] ModelUpdateName model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
           
            var response = _authService.CheckAccessAdminOrPerformingUser(model.Token, model.UserLogin);
            if (response.StatusCode != AtonWebApi.Response.StatusCode.Ok)
            {
                return StatusCode((int)response.StatusCode, response.Description);
            }
            var modifiedBy = _authService.GetUserLogin(model.Token);
            response = await _usersService.UpdateNameAsync(modifiedBy, model.UserLogin, model.NewUserName);
            return StatusCode((int)response.StatusCode, response.Description);
        }
        [HttpPatch("UpdateBirthday")]
        public async Task<ActionResult> UpdateBirthdayAsync([FromBody] ModelUpdateBirthday model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var response = _authService.CheckAccessAdminOrPerformingUser(model.Token, model.UserLogin);
            if (response.StatusCode != AtonWebApi.Response.StatusCode.Ok)
            {
                return StatusCode((int)response.StatusCode, response.Description);
            }
            var modifiedBy = _authService.GetUserLogin(model.Token);
            response = await _usersService.UpdateBirthdayAsync(modifiedBy, model.UserLogin, model.NewBirthday);
            return StatusCode((int)response.StatusCode, response.Description);
        }
        [HttpPatch("UpdateGender")]
        public async Task<ActionResult> UpdateGenderAsync([FromBody] ModelUpdateGender model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var response = _authService.CheckAccessAdminOrPerformingUser(model.Token, model.UserLogin);
            if (response.StatusCode != AtonWebApi.Response.StatusCode.Ok)
            {
                return StatusCode((int)response.StatusCode, response.Description);
            }
            var modifiedBy = _authService.GetUserLogin(model.Token);
            response = await _usersService.UpdateGenderAsync(modifiedBy, model.UserLogin, model.NewGender);
            return StatusCode((int)response.StatusCode, response.Description);
        }
        [HttpGet("GetActiveUsers")]
        public async Task<ActionResult<User[]>> GetActiveUsersAsync(string token)
        {

            var response = _authService.VerifyAdmin(token);
            if (response.StatusCode != AtonWebApi.Response.StatusCode.Ok)
            {
                return StatusCode((int)response.StatusCode, response.Description);
            }
            response = await _usersService.GetActiveUsersAsync();
            var result = ((BaseResponse<User[]>)response).Data;
            return Ok(result);
            
        }
        [HttpGet("GetUserForAdmin")]
        public ActionResult GetUserDataForAdmin(string token, string userLogin)
        {
            var response = _authService.VerifyAdmin(token);
            if (response.StatusCode != AtonWebApi.Response.StatusCode.Ok)
            {
                return StatusCode((int)response.StatusCode, response.Description);
            }
             response =  _usersService.GetUserDataForAdmin(userLogin);
            if (response.StatusCode != AtonWebApi.Response.StatusCode.Ok)
            {
                return StatusCode((int)response.StatusCode, response.Description);
            }
            var user = ((BaseResponse<User>)response).Data;
            return Ok(new
            {
                Name = user.Name,
                Gender = user.Gender,
                Birthday = user.Birthday,
                Active = user.RevokedOn == null ? true : false
            });
        }
        [HttpGet("GetUserData")]
        public async Task<ActionResult<User>> GetUserDataAsync(string token)
        {
            if (!_authService.IsValidToken(token))
            {
                return StatusCode(403, "Invalid token");
            }
            var login = _authService.GetUserLogin(token);
            var response = await _usersService.GetUserDataAsync(login);
            if (response.StatusCode != AtonWebApi.Response.StatusCode.Ok)
            {
                return StatusCode((int)response.StatusCode,response.Description);
            }
            var result = ((BaseResponse<User>)response).Data;
            return Ok(result);
        }
        [HttpGet("GetUsersByAge")]
        public async Task<ActionResult<User[]>> GetUsersByAgeAsync(string token, int age)
        {
            var response = _authService.VerifyAdmin(token);
            if (response.StatusCode != AtonWebApi.Response.StatusCode.Ok)
            {
                return StatusCode((int)response.StatusCode, response.Description);
            }
            response = await _usersService.GetUsersByAgeAsync(age);
            var result = ((BaseResponse<User[]>)response).Data;
            if (result.Length == 0)
            {
                return Ok("These users weren't found");
            }
            return Ok(result);
        }
        [HttpDelete("DeleteUser")]
        public async Task<ActionResult> DeleteUserAsync([FromBody] ModelLoginOperation model)
        {
            var response = _authService.VerifyAdmin(model.Token);
            if (response.StatusCode != AtonWebApi.Response.StatusCode.Ok)
            {
                return StatusCode((int)response.StatusCode, response.Description);
            }
            response = await _usersService.DeleteUserAsync(model.UserLogin);
            if (response.StatusCode != AtonWebApi.Response.StatusCode.Ok)
            {
                return StatusCode((int)response.StatusCode, response.Description);
            }
            return Ok(response.Description);

        }
        [HttpPatch("RevokeUser")]
        public async Task<ActionResult> RevokeUser([FromBody] ModelLoginOperation model)
        {
            var response = _authService.VerifyAdmin(model.Token);
            if (response.StatusCode != AtonWebApi.Response.StatusCode.Ok)
            {
                return StatusCode((int)response.StatusCode, response.Description);
            }
            var revokedBy = _authService.GetUserLogin(model.Token);
            response = await _usersService.RevokeUserAsync(revokedBy, model.UserLogin);
            if (response.StatusCode != AtonWebApi.Response.StatusCode.Ok)
            {
                return StatusCode((int)response.StatusCode, response.Description);
            }
            return Ok(response.Description);
        }
        [HttpPatch("UserRecovery")]
        public async Task<ActionResult> UserRecoveryAsync([FromBody] ModelLoginOperation model)
        {
            var response = _authService.VerifyAdmin(model.Token);
            if (response.StatusCode != AtonWebApi.Response.StatusCode.Ok)
            {
                return StatusCode((int)response.StatusCode, response.Description);
            }
            response = await _usersService.RecoveryUserAsync( model.UserLogin);
            if (response.StatusCode != AtonWebApi.Response.StatusCode.Ok)
            {
                return StatusCode((int)response.StatusCode, response.Description);
            }
            return Ok(response.Description);
        }
    }
}
