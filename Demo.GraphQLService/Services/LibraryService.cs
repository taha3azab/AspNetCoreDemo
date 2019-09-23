using Demo.GraphQLService.Data.Entities;
using Demo.GraphQLService.Dtos;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.GraphQLService.Services
{
    public interface ILibraryService
    {
        Task<PagedListDto<BookDto>> GetBooksAsync(int pageIndex = 0, int pageSize = 20);
        Task<BookDto> AddBookAsync(BookDto bookDto);
    }

    public class LibraryService : ILibraryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public LibraryService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<BookDto> AddBookAsync(BookDto bookDto)
        {
            var book = bookDto.Adapt<Book>();
            await _unitOfWork.GetRepository<Book>().InsertAsync(book);
            await _unitOfWork.SaveChangesAsync();
            return book.Adapt<BookDto>();
        }

        public async Task<PagedListDto<BookDto>> GetBooksAsync(int pageIndex = 0, int pageSize = 20)
        {
            pageIndex = pageIndex < 0 ? 0 : pageIndex;
            pageSize = pageSize < 1 ? 1 : pageSize;

            var books = await _unitOfWork.GetRepository<Book>()
                                         .GetPagedListAsync(null,
                                                            q => q.OrderBy(u => u.Id),
                                                            q => q.Include(u => u.Authors),
                                                            pageIndex,
                                                            pageSize);
            return books?.Adapt<PagedListDto<BookDto>>();
        }
    }
}
