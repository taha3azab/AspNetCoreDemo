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
        private readonly IAuthService _authService;
        private readonly IConfiguration _config;
        public AuthController(IAuthService authService, IConfiguration configuration)
        {
            _authService = authService;
            _config = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegister)
        {
            userForRegister.Username = userForRegister.Username.ToLower();
            if (await _authService.UserExists(userForRegister.Username))
                return BadRequest("Username already exists");

            var userToCreate = new User
            {
                Username = userForRegister.Username
            };
            var createdUser = await _authService.Register(userToCreate, userForRegister.Password);
            return StatusCode(StatusCodes.Status201Created);
            //return CreatedAtRoute("", createdUser);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserForLoginDto userForLogin)
        {
            var user = await _authService.Login(userForLogin.Username.ToLower(), userForLogin.Password);
            if (user == null)
                return Unauthorized();

            var secret = _config.GetSection("AppSettings:Secret").Value;
            var token = new JwtBuilder()
                            .WithAlgorithm(new HMACSHA256Algorithm())
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

        [HttpPost("change_password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(UserForChangePasswordDto userForChangePassword)
        {
            var user = await _authService.ChangePassword(userForChangePassword.Username, userForChangePassword.OldPassword, userForChangePassword.NewPassword);
            if (user == null)
                return Unauthorized();
            return StatusCode(StatusCodes.Status202Accepted);
        }
    }
}