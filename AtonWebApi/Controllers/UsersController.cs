
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
        public async Task<ActionResult<User>> CreateUser([FromBody] ModelForOperation model)
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

            //if (model.Login != model.User.Login&&user.Admin!=true)
            //{
            //    return StatusCode(403, "Only the user or the administrator can make changes to the user's properties");
            //}

            var result = await _userRepository.GetUserAsync(model.User.Login);

            if (result != null)
            {
                return BadRequest("User with this login already exists");
            }

            var newUser = new User(model.User,model.Login);
            await _userRepository.CreateUserAsync(newUser);            
           
            return Ok("User is added");
        }

    }
}
