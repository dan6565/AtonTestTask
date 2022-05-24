
using AtonTestTask.Data.Repositories;
using AtonWebApi.Dto;
using AtonWebApi.Models;
using Microsoft.AspNetCore.Http;
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
        public async Task<ActionResult<User>> CreateUser([FromBody] UserDto user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _userRepository.GetUserAsync(user.Login);
            if (result != null)
            {
                return BadRequest("User with this login already exists");
            }
            var newUser = new User(user,"admin");
            await _userRepository.CreateUserAsync(newUser);
            
           
            return Ok("User is added");
        }

    }
}
