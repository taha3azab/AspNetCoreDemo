using Demo.API.Dtos;
using Demo.API.Models;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

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
            pageIndex = pageIndex < 0 ? 0 : pageIndex;
            pageSize = pageSize < 1 ? 1 : pageSize;
            var user = this.User;

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
            if (user == null)
                return NotFound();
            return Ok(user);
        }
    }
}