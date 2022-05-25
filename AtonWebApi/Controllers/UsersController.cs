
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
            await _userRepository.UpdateUser(targetUser);
            return Ok("Login seccessfully changed");

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
            var targetUser = await _userRepository.GetUserAsync(model.UserLogin,model.PreviousUserPassword);
            if (targetUser == null)
            {
                return BadRequest("The username or password of the user being changed is incorrect");
            }
            targetUser.Password = model.NewUserPassword;
            targetUser.ModifiedBy = model.Login;
            targetUser.ModifiedOn = DateTime.Now;
            await _userRepository.UpdateUser(targetUser);
            return Ok("Password seccessfully changed");
        }

    }
}
