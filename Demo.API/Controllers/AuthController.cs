using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Demo.API.Data;
using Demo.API.Dtos;
using Demo.API.Helpers;
using Demo.API.Models;
using JWT;
using JWT.Algorithms;
using JWT.Builder;
using JWT.Serializers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Demo.API.Controllers
{
    [ApiVersion("2.0")]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepo;
        private readonly IConfiguration _config;
        public AuthController(IAuthRepository authRepository, IConfiguration configuration)
        {
            _authRepo = authRepository;
            _config = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegister)
        {
            userForRegister.Username = userForRegister.Username.ToLower();
            if (await _authRepo.UserExists(userForRegister.Username))
                return BadRequest("Username already exists");

            var userToCreate = new User
            {
                Username = userForRegister.Username
            };
            var createdUser = await _authRepo.Register(userToCreate, userForRegister.Password);
            return StatusCode(StatusCodes.Status201Created);
            //return CreatedAtRoute("", createdUser);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserForLoginDto userForLogin)
        {
            var user = await _authRepo.Login(userForLogin.Username.ToLower(), userForLogin.Password);
            if (user == null)
                return Unauthorized();

            // var claims = new[]
            // {
            //     new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            //     new Claim(ClaimTypes.Name, user.Username)
            // };

            // var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
            // var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            // var tokenDescriptor = new SecurityTokenDescriptor
            // {
            //     Subject = new ClaimsIdentity(claims),
            //     Expires = DateTime.Now.AddDays(1),
            //     SigningCredentials = cred
            // };

            // var tokenHandler = new JwtSecurityTokenHandler();
            // var token = tokenHandler.CreateToken(tokenDescriptor);

            // return Ok(new
            // {
            //     token = tokenHandler.WriteToken(token)
            // });
            var secret = _config.GetSection("AppSettings:Secret").Value;
            var token = new JwtBuilder()
                            .WithDateTimeProvider(new UtcDateTimeProvider())
                            .WithAlgorithm(new HMACSHA256Algorithm())
                            .WithSerializer(new JsonNetSerializer())
                            .WithSecret(secret)
                            .AddClaim(JwtRegisteredClaimNames.NameId, user.Id.ToString())
                            .AddClaim(JwtRegisteredClaimNames.UniqueName, user.Username)
                            .AddClaim(JwtRegisteredClaimNames.AuthTime, DateTime.UtcNow.ToString())
                            .AddClaim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString())
                            .AddClaim(JwtRegisteredClaimNames.Nbf, DateTime.UtcNow.ToString())
                            .AddClaim(JwtRegisteredClaimNames.Exp, DateTime.UtcNow.AddDays(1).ToString())
                            // .IssuedAt(DateTime.Now)
                            // .NotBefore(DateTime.Now)
                            // .ExpirationTime(DateTime.Now.AddDays(1))
                            .WithVerifySignature(true)
                            .Build();
            return Ok(new
            {
                token = token
            });

        }

        [Authorize]
        [HttpPost("change_password")]
        public async Task<IActionResult> ChangePassword(UserForChangePasswordDto userForChangePassword)
        {
            var user = await _authRepo.ChangePassword(userForChangePassword.Username, userForChangePassword.OldPassword, userForChangePassword.NewPassword);
            if (user == null)
                return Unauthorized();
            return StatusCode(StatusCodes.Status202Accepted);
        }
    }
}