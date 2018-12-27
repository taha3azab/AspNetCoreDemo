using System.Threading.Tasks;
using Demo.API.Models;

namespace Demo.API.Data
{
    public interface IAuthService
    {
        Task<User> Register(User user, string password);
        Task<User> Login(string username, string password);
        Task<bool> UserExists(string username);
        Task<User> ChangePassword(string username, string oldPassword, string newPassword);
    }
}