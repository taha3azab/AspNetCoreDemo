
using Demo.API.Dtos;
using Demo.API.Models;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.API.Services
{
    public interface IUsersService
    {
        Task<PagedListDto<UserForListDto>> GetUsers(int pageIndex, int pageSize);
        Task<UserForDetailedDto> GetUserById(int id);
    }

    public class UsersService : IUsersService
    {
        private readonly IUnitOfWork _unitOfWork;
        public UsersService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;
        public async Task<PagedListDto<UserForListDto>> GetUsers(int pageIndex = 0, int pageSize = 20)
        {
            pageIndex = pageIndex < 0 ? 0 : pageIndex;
            pageSize = pageSize < 1 ? 1 : pageSize;

            var users = await _unitOfWork.GetRepository<User>().GetPagedListAsync(null,
                                                                        q => q.OrderBy(u => u.Id),
                                                                        q => q.Include(u => u.Photos),
                                                                        pageIndex,
                                                                        pageSize);
            return users?.Adapt<PagedListDto<UserForListDto>>();
        }
        public async Task<UserForDetailedDto> GetUserById(int id)
        {
            return await _unitOfWork.GetRepository<User>()
                                    .GetFirstOrDefaultAsync(
                                        u => u.Adapt<UserForDetailedDto>(),
                                        u => u.Id == id,
                                        q => q.OrderBy(u => u.Id),
                                        q => q.Include(u => u.Photos));
        }
    }
}