using Demo.API.Dtos;
using Demo.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Demo.API.Controllers
{
    // [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedListDto<UserForListDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUsers(int pageIndex = 0, int pageSize = 20)
        {
            return Ok(await _usersService.GetUsers(pageIndex, pageSize));
        }
        
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserForDetailedDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUsers(int id)
        {
            var user = await _usersService.GetUserById(id) ;
            if (user == null)
                return NotFound();
            return Ok(user);
        }
    }
}