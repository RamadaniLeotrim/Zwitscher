using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Zwitscher.Models
{
    public class User : IdentityUser
    {
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public class EditModel
    {
        [Required(ErrorMessage = "Benutzername ist erforderlich")]
        public string? UserName { get; set; }

        [MinLength(6, ErrorMessage = "Das Passwort braucht mindestens 6 Zeichen.")]
        public string? Password { get; set; }
    }
}
