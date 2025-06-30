using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Zwitscher.Models
{
    public class User : IdentityUser
    {
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
