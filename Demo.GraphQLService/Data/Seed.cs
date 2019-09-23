using Demo.GraphQLService.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo.GraphQLService.Data
{
    public class Seed
    {
        private readonly IUnitOfWork _unitOfWork;
        public Seed(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task SeedLibrary()
        {
            var storedBooks = await _unitOfWork.GetRepository<Book>().GetPagedListAsync();
            _unitOfWork.GetRepository<Book>().Delete(storedBooks.Items);
            await _unitOfWork.SaveChangesAsync();

            var booksData = await System.IO.File.ReadAllTextAsync("Data/LibrarySeedData.json");
            var books = JsonConvert.DeserializeObject<List<Book>>(booksData);
            

            await _unitOfWork.GetRepository<Book>().InsertAsync(books);
            await _unitOfWork.SaveChangesAsync();
        }

    }
}