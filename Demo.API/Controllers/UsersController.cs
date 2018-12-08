using System.Linq;
using System.Threading.Tasks;
using Demo.API.Dtos;
using Demo.API.Models;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public UsersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedListDto<UserForListDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUsers(int pageIndex = 0, int pageSize = 20)
        {
            var users = await _unitOfWork.GetRepository<User>().GetPagedListAsync(null,
                                                                                    q => q.OrderBy(u => u.Id),
                                                                                    q => q.Include(u => u.Photos),
                                                                                    pageIndex,
                                                                                    pageSize);
            return Ok(users?.Adapt<PagedListDto<UserForListDto>>());
        }
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserForDetailedDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUsers(int id)
        {
            var user = await _unitOfWork.GetRepository<User>().GetFirstOrDefaultAsync(u => u.Adapt<UserForDetailedDto>(),
                                                                                        u => u.Id == id,
                                                                                        q => q.OrderBy(u => u.Id),
                                                                                        q => q.Include(u => u.Photos));
            return Ok(user);
        }
    }
}