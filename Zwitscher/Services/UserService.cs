using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Zwitscher.Data;
using Zwitscher.Models;

namespace Zwitscher.Services
{
    public class UserService : IUserService
    {
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly UserManager<User> _userManager;

        public UserService(
            AuthenticationStateProvider authStateProvider,
            UserManager<User> userManager)
        {
            _authStateProvider = authStateProvider;
            _userManager = userManager;
        }

        public async Task<bool> IsAuthenticatedAsync()
        {
            var authState = await _authStateProvider.GetAuthenticationStateAsync();
            return authState.User.Identity?.IsAuthenticated ?? false;
        }

        public async Task<string?> GetUserNameAsync()
        {
            var authState = await _authStateProvider.GetAuthenticationStateAsync();
            return authState.User.Identity?.Name;
        }

        public async Task<User?> GetUserAsync()
        {
            var authState = await _authStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            if (user.Identity?.IsAuthenticated != true)
                return null;
            return await _userManager.GetUserAsync(user);
        }

        public async Task<IList<string>> GetRolesAsync()
        {
            var appUser = await GetUserAsync();
            if (appUser == null)
                return new List<string>();
            return await _userManager.GetRolesAsync(appUser);
        }
    }

}
