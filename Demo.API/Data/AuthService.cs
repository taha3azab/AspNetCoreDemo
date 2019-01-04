using Demo.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.API.Data
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        public AuthService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<User> Login(string username, string password)
        {
            var user = await _unitOfWork.GetRepository<User>()
                    .GetFirstOrDefaultAsync(u => u, u => u.Username == username);

            if (user == null)
                return null;

            return !VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt) ? null : user;
        }
        public async Task<User> Register(User user, string password)
        {
            CreatePasswordHash(password, out var passwordHash, out var passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _unitOfWork.GetRepository<User>().InsertAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return user;
        }
        public async Task<bool> UserExists(string username)
        {
            return await Task.Run(() => _unitOfWork.GetRepository<User>().Count(u => u.Username.ToLower() == username.ToLower()) > 0);
        }
        public async Task<User> ChangePassword(string username, string oldPassword, string newPassword)
        {
            var user = await Login(username, oldPassword);
            if (user == null)
                return null;

            CreatePasswordHash(newPassword, out var passwordHash, out var passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _unitOfWork.GetRepository<User>().Update(user);
            await _unitOfWork.SaveChangesAsync();

            return user;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            // https://crackstation.net/hashing-security.htm
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return passwordHash != null && 
                        computedHash.SequenceEqual(passwordHash);
            }
        }
    }
}