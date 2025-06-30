using Zwitscher.Models;

namespace Zwitscher.Services
{
    public interface IUserService
    {
        public interface IUserService
        {
            Task<bool> IsAuthenticatedAsync();
            Task<string?> GetUserNameAsync();
            Task<User?> GetUserAsync();
            Task<IList<string>> GetRolesAsync();
        }
    }
}
