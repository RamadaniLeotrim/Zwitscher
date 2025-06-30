// Controllers/AuthController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Zwitscher.Models;  // ApplicationUser
using System.ComponentModel.DataAnnotations;

namespace Zwitscher.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<User> _signInMgr;
        private readonly UserManager<User> _userMgr;
        private readonly RoleManager<IdentityRole> _roleMgr;

        public AuthController(SignInManager<User> signInMgr, UserManager<User> userMgr, RoleManager<IdentityRole> roleMgr)
        {
            _signInMgr = signInMgr;
            _userMgr = userMgr;
            _roleMgr = roleMgr;
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] LoginDto input)
        {
            var result = await _signInMgr.PasswordSignInAsync(
                input.Username, input.Password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
                return Redirect("/");
            return Redirect("/login?error=1");
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInMgr.SignOutAsync();
            return Redirect("/");
        }

        // Neuer Register-Endpoint
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] RegisterDto input)
        {
            // 1) Model-State prüfen (inkl. Compare-Attribute)
            if (!ModelState.IsValid)
                return Redirect("/register?error=validation");

            // 2) Doppelten Benutzernamen verhindern
            if (await _userMgr.FindByNameAsync(input.Username) is not null)
                return Redirect("/register?error=duplicate");

            // 3) User anlegen
            var user = new User
            {
                UserName = input.Username,
                CreatedAt = DateTime.UtcNow
            };

            var createResult = await _userMgr.CreateAsync(user, input.Password);
            if (!createResult.Succeeded)
                return Redirect("/register?error=creation");

            // 4) Rolle "User" anlegen, falls noch nicht vorhanden
            const string userRole = "User";
            if (!await _roleMgr.RoleExistsAsync(userRole))
                await _roleMgr.CreateAsync(new IdentityRole(userRole));

            // 5) Rolle zuweisen
            await _userMgr.AddToRoleAsync(user, userRole);

            // 6) Automatisches Einloggen
            await _signInMgr.SignInAsync(user, isPersistent: false);

            // 7) Erfolgs-Redirect
            return Redirect("/");
        }
    }
}
