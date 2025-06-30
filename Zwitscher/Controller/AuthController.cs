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
        public AuthController(SignInManager<User> signInMgr)
        {
            _signInMgr = signInMgr;
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
    }



}
