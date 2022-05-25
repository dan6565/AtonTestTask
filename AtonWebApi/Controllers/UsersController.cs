
using AtonTestTask.Data.Repositories;
using AtonWebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace AtonWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UsersRepository _userRepository;
       
        public UsersController(UsersRepository usersRepository) { _userRepository = usersRepository; }
        [HttpPost("create")]
        public async Task<ActionResult> CreateUser([FromBody] ModelForCreating model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userRepository.GetUserAsync(model.Login, model.Password);
            if (user == null)
            {
                return StatusCode(403,"Invalid login or password");
            }
            if (user.Admin != true)
            {
                return StatusCode(403, "Only administrator can create users");
            }

            var result = await _userRepository.GetUserAsync(model.User.Login);

            if (result != null)
            {
                return BadRequest("User with this login already exists");
            }

            var newUser = new User(model.User,model.Login);
            await _userRepository.CreateUserAsync(newUser);            
           
            return Ok("User is added");
        }
        [HttpPatch("updatelogin")]
        public async Task<ActionResult> UpdateLogin([FromBody] ModelForUpdateLogin model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await _userRepository.GetUserAsync(model.Login, model.Password);
            if (user == null)
            {
                return StatusCode(403, "Invalid login or password");
            }
            if(user.Admin != true)
            {
                if (model.Login != model.PreviousUserLogin)
                {
                    return StatusCode(403, "Only the user or the administrator can make changes to the user's properties");
                }
                if (user.RevokedOn!=null)
                {
                    return StatusCode(403, "The rights of the user performing the operation have been revoked");
                }
            }
            var targetUser = await _userRepository.GetUserAsync(model.PreviousUserLogin);
            if (targetUser == null)
            {
                return NotFound("User with this login doesn't exist");
            }
            targetUser.Login = model.NewUserLogin;
            targetUser.ModifiedBy = model.Login;
            targetUser.ModifiedOn = DateTime.Now;
            await _userRepository.UpdateUserAsync(targetUser);
            return Ok("Login has been successfully changed");

        }
        [HttpPatch("updatepassword")]
        public async Task<ActionResult> UpdatePassword([FromBody] ModelForUpdatePassword model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await _userRepository.GetUserAsync(model.Login, model.Password);
            if (user == null)
            {
                return StatusCode(403, "Invalid login or password");
            }
            if (user.Admin != true)
            {
                if (model.Login != model.UserLogin)
                {
                    return StatusCode(403, "Only the user or the administrator can make changes to the user's properties");
                }
                if (user.RevokedOn != null)
                {
                    return StatusCode(403, "The rights of the user performing the operation have been revoked");
                }
            }
            var targetUser = await _userRepository.GetUserAsync(model.UserLogin);
            if (targetUser == null)
            {
                return NotFound("User with this login doesn't exist");
            }
            targetUser.Password = model.NewUserPassword;
            targetUser.ModifiedBy = model.Login;
            targetUser.ModifiedOn = DateTime.Now;
            await _userRepository.UpdateUserAsync(targetUser);
            return Ok("Password has been successfully changed");
        }
        [HttpPatch("updatename")]
        public async Task<ActionResult> UpdateName([FromBody] ModelForUpdateName model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await _userRepository.GetUserAsync(model.Login, model.Password);
            if (user == null)
            {
                return StatusCode(403, "Invalid login or password");
            }
            if (user.Admin != true)
            {
                if (model.Login != model.UserLogin)
                {
                    return StatusCode(403, "Only the user or the administrator can make changes to the user's properties");
                }
                if (user.RevokedOn != null)
                {
                    return StatusCode(403, "The rights of the user performing the operation have been revoked");
                }
            }
            var targetUser = await _userRepository.GetUserAsync(model.UserLogin);
            if (targetUser == null)
            {
                return NotFound("User with this login doesn't exist");
            }
            targetUser.Name = model.NewUserName;
            targetUser.ModifiedBy = model.Login;
            targetUser.ModifiedOn = DateTime.Now;
            await _userRepository.UpdateUserAsync(targetUser);
            return Ok($"{model.UserLogin}'s name has been successfully chenged");
        }
        [HttpPatch("updatebirthday")]
        public async Task<ActionResult> UpdateBirthday([FromBody] ModelForUpdateBirthday model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await _userRepository.GetUserAsync(model.Login, model.Password);
            if (user == null)
            {
                return StatusCode(403, "Invalid login or password");
            }
            if (user.Admin != true)
            {
                if (model.Login != model.UserLogin)
                {
                    return StatusCode(403, "Only the user or the administrator can make changes to the user's properties");
                }
                if (user.RevokedOn != null)
                {
                    return StatusCode(403, "The rights of the user performing the operation have been revoked");
                }
            }
            var targetUser = await _userRepository.GetUserAsync(model.UserLogin);
            if (targetUser == null)
            {
                return NotFound("User with this login doesn't exist");
            }
            targetUser.Birthday = model.NewBirthday;
            targetUser.ModifiedBy = model.Login;
            targetUser.ModifiedOn = DateTime.Now;
            await _userRepository.UpdateUserAsync(targetUser);
            return Ok($"{model.UserLogin}'s birthday has been successfully changed");
        }
        [HttpPatch("updategender")]
        public async Task<ActionResult> UpdateGender([FromBody] ModelForUpdateGender model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await _userRepository.GetUserAsync(model.Login, model.Password);
            if (user == null)
            {
                return StatusCode(403, "Invalid login or password");
            }
            if (user.Admin != true)
            {
                if (model.Login != model.UserLogin)
                {
                    return StatusCode(403, "Only the user or the administrator can make changes to the user's properties");
                }
                if (user.RevokedOn != null)
                {
                    return StatusCode(403, "The rights of the user performing the operation have been revoked");
                }
            }
            var targetUser = await _userRepository.GetUserAsync(model.UserLogin);
            if (targetUser == null)
            {
                return NotFound("User with this login doesn't exist");
            }
            targetUser.Gender = model.NewGender;
            targetUser.ModifiedBy = model.Login;
            targetUser.ModifiedOn = DateTime.Now;
            await _userRepository.UpdateUserAsync(targetUser);
            return Ok($"{model.UserLogin}'s gender has been successfully changed");
        }
        [HttpGet("GetActiveUser")]
        public async Task<ActionResult<User[]>> GetActiveUsers(string adminLogin, string password)
        {
            var user = await _userRepository.GetUserAsync(adminLogin, password);
            if (user == null)
            {
                return BadRequest("Invalid login or password");
            }
            if (user.Admin == false)
            {
                return BadRequest("Only admin can reсeive list of users");
            }
            var result = await _userRepository.GetActiveUsersAsync();
            return Ok(result);
            
        }
        [HttpGet("GetUserForAdmin")]
        public async Task<ActionResult<User[]>> GetUser(string adminLogin, string password, string userLogin)
        {
            var user = await _userRepository.GetUserAsync(adminLogin, password);
            if (user == null)
            {
                return BadRequest("Invalid login or password");
            }
            if (user.Admin == false)
            {
                return BadRequest("Only admin can reсeive data of users");
            }
            var result = await _userRepository.GetUserAsync(userLogin);
            return Ok(new
            {
                Name = result.Name,
                Gender = result.Gender,
                Birthday = result.Birthday,
                Active = result.RevokedOn == null ? true : false
            });
        }
        [HttpGet("GetUserData")]
        public async Task<ActionResult<User>> GetUserData(string login, string password)
        {
            var user = await _userRepository.GetUserAsync(login, password);
            if (user == null)
            {
                return BadRequest("Invalid login or password");
            }
            if (user.RevokedOn != null)
            {
                return StatusCode(403, "The rights of the user performing the operation have been revoked");
            }
            return Ok(user);
        }
        [HttpGet("GetUsersByAge")]
        public async Task<ActionResult<User[]>> GetUsersByAge(string adminLogin,string password,int age)
        {
            var user = await _userRepository.GetUserAsync(adminLogin, password);
            if (user == null)
            {
                return BadRequest("Invalid login or password");
            }
            if (user.Admin == false)
            {
                return BadRequest("Only admin can reсeive data of users");
            }
            var result = await _userRepository.GetUsersByAgeAsync(age);
            if (result.Length == 0)
            {
                return Ok("These users weren't found");
            }
            return Ok(result);
        }
        [HttpDelete("DeleteUser")]
        public async Task<ActionResult> DeleteUser(string adminLogin,string password, string userLogin)
        {
            var user = await _userRepository.GetUserAsync(adminLogin, password);
            if (user == null)
            {
                return BadRequest("Invalid login or password");
            }
            if (user.Admin == false)
            {
                return StatusCode(403, "Only admin can delete users");
            }
            var result = await _userRepository.GetUserAsync(userLogin);
            if (result == null)
            {
                return NotFound("User with this login don't exists");
            }
            await _userRepository.DeleteUserAsync(result);
            return Ok("User has been successfully deleted");

        }
        [HttpPatch("RevokeUser")]
        public async Task<ActionResult> RevokeUser(string adminLogin, string password, string userLogin)
        {
            var user = await _userRepository.GetUserAsync(adminLogin, password);
            if (user == null)
            {
                return BadRequest("Invalid login or password");
            }
            if (user.Admin == false)
            {
                return StatusCode(403, "Only admin can revoke users");
            }
            var result = await _userRepository.GetUserAsync(userLogin);
            if (result == null)
            {
                return NotFound("User with this login don't exists");
            }
            result.RevokedBy = adminLogin;
            result.RevokedOn = DateTime.Now;
            await _userRepository.UpdateUserAsync(result);
            return Ok("User has been successfully revoked");
        }
        [HttpPatch("UserRecovery")]
        public async Task<ActionResult> UserRecovery(string adminLogin, string password, string userLogin)
        {
            var user = await _userRepository.GetUserAsync(adminLogin, password);
            if (user == null)
            {
                return BadRequest("Invalid login or password");
            }
            if (user.Admin == false)
            {
                return StatusCode(403, "Only admin can recovery users");
            }
            var result = await _userRepository.GetUserAsync(userLogin);
            if (result == null)
            {
                return NotFound("User with this login don't exists");
            }
            result.RevokedBy = null;
            result.RevokedOn = null;
            await _userRepository.UpdateUserAsync(result);
            return Ok("User has been successfully recovered");
        }
    }
}
