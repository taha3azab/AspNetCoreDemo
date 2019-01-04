using Demo.API.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Demo.API.Data
{
    public class Seed
    {
        private readonly IUnitOfWork _unitOfWork;
        public Seed(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task SeedUsers()
        {
            var userData = await System.IO.File.ReadAllTextAsync("Data/UserSeedData.json");
            var users = JsonConvert.DeserializeObject<List<User>>(userData);
            foreach (var user in users)
            {
                CreatePasswordHash("password", out var passwordHash, out var passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                user.Username = user.Username.ToLower();

                await _unitOfWork.GetRepository<User>().InsertAsync(user);

            }

            await _unitOfWork.SaveChangesAsync();
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}