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

        public UsersController(IUsersRepository usersRepository,UsersService usersService) 
        { 
            _usersService = usersService;
        }
        [HttpPost("Create")]
        public async Task<ActionResult> CreateUserAsync([FromBody] ModelCreating model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _usersService.CreateUser(model.Login, model.Password, model.User);

            return StatusCode((int)response.StatusCode, response.Description);

        }
        [HttpPatch("UpdateLogin")]
        public async Task<ActionResult> UpdateLoginAsync([FromBody] ModelUpdateLogin model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _usersService.UpdateLogin(model.Login, model.Password, model.PreviousUserLogin, model.NewUserLogin);
            return StatusCode((int)response.StatusCode, response.Description);
        }
        [HttpPatch("UpdatePassword")]
        public async Task<ActionResult> UpdatePasswordAsync([FromBody] ModelUpdatePassword model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var response = await _usersService.UpdatePasswordAsync(model.Login, model.Password, model.UserLogin, model.NewUserPassword);
            return StatusCode((int)response.StatusCode, response.Description);
        }
        [HttpPatch("UpdateName")]
        public async Task<ActionResult> UpdateNameAsync([FromBody] ModelUpdateName model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _usersService.UpdateNameAsync(model.Login, model.Password, model.UserLogin, model.NewUserName);
            return StatusCode((int)response.StatusCode, response.Description);
        }
        [HttpPatch("UpdateBirthday")]
        public async Task<ActionResult> UpdateBirthdayAsync([FromBody] ModelUpdateBirthday model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _usersService.UpdateBirthdayAsync(model.Login, model.Password, model.UserLogin, model.NewBirthday);
            return StatusCode((int)response.StatusCode, response.Description);
        }
        [HttpPatch("UpdateGender")]
        public async Task<ActionResult> UpdateGenderAsync([FromBody] ModelUpdateGender model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _usersService.UpdateGenderAsync(model.Login, model.Password, model.UserLogin, model.NewGender);
            return StatusCode((int)response.StatusCode, response.Description);
        }
        [HttpGet("GetActiveUsers")]
        public async Task<ActionResult<User[]>> GetActiveUsersAsync(string adminLogin,string password)
        {
            var response = await _usersService.GetActiveUsersAsync(adminLogin, password);
            if (response.StatusCode != AtonWebApi.Response.StatusCode.Ok)
            {
                return StatusCode((int)response.StatusCode, response.Description);
            }
            var result = ((BaseResponse<User[]>)response).Data;
            return Ok(result);
            
        }
        [HttpGet("GetUserForAdmin")]
        public ActionResult GetUserDataForAdminAsync(string adminLogin,string password,string userLogin)
        {

            var response =  _usersService.GetUserDataForAdmin(adminLogin, password, userLogin);
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
        public ActionResult<User> GetUserData(string login,string password)
        {
            var response = _usersService.GetUserData(login, password);
            if (response.StatusCode != AtonWebApi.Response.StatusCode.Ok)
            {
                return StatusCode((int)response.StatusCode,response.Description);
            }
            var result = ((BaseResponse<User>)response).Data;
            return Ok(result);
        }
        [HttpGet("GetUsersByAge")]
        public async Task<ActionResult<User[]>> GetUsersByAge( string adminLogin,string password, int age)
        {
            var response = await _usersService.GetUsersByAgeAsync(adminLogin, password,age);
            if (response.StatusCode != AtonWebApi.Response.StatusCode.Ok)
            {
                return StatusCode((int)response.StatusCode, response.Description);
            }
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
            var response = await _usersService.DeleteUserAsync(model.AdminLogin, model.Password, model.UserLogin);
            if (response.StatusCode != AtonWebApi.Response.StatusCode.Ok)
            {
                return StatusCode((int)response.StatusCode, response.Description);
            }
            return Ok(response.Description);

        }
        [HttpPatch("RevokeUser")]
        public async Task<ActionResult> RevokeUser([FromBody] ModelLoginOperation model)
        {
            var response = await _usersService.RevokeUserAsync(model.AdminLogin, model.Password, model.UserLogin);
            if (response.StatusCode != AtonWebApi.Response.StatusCode.Ok)
            {
                return StatusCode((int)response.StatusCode, response.Description);
            }
            return Ok(response.Description);
        }
        [HttpPatch("UserRecovery")]
        public async Task<ActionResult> UserRecoveryAsync([FromBody] ModelLoginOperation model)
        {
            var response = await _usersService.RecoveryUserAsync(model.AdminLogin, model.Password, model.UserLogin);
            if (response.StatusCode != AtonWebApi.Response.StatusCode.Ok)
            {
                return StatusCode((int)response.StatusCode, response.Description);
            }
            return Ok(response.Description);
        }
    }
}
