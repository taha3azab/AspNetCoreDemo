using System.Threading.Tasks;
using Demo.API.Data;
using Demo.API.Dtos;
using Demo.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Demo.API.Controllers
{
    [ApiVersion("2.0")]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]UserForRegisterDto userForRegister)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            userForRegister.Username = userForRegister.Username.ToLower();
            if (await _authRepository.UserExists(userForRegister.Username))
                return BadRequest("Username already exists");

            var userToCreate = new User
            {
                Username = userForRegister.Username
            };
            var createdUser = await _authRepository.Register(userToCreate, userForRegister.Password);
            return StatusCode(StatusCodes.Status201Created);
            //return CreatedAtRoute("", createdUser);
        }


    }
}