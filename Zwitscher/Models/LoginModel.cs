﻿using System.ComponentModel.DataAnnotations;

namespace Zwitscher.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Benutzername ist erforderlich")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Passwort ist erforderlich")]
        public string Password { get; set; } = string.Empty;
    }

    public class LoginDto
    {
        [Required] public string Username { get; set; }
        [Required] public string Password { get; set; }
    }

    public class RegisterDto
    {
        [Required]
        public string Username { get; set; } = "";

        [Required, DataType(DataType.Password), MinLength(6)]
        public string Password { get; set; } = "";

        [Required, DataType(DataType.Password), Compare(nameof(Password), ErrorMessage = "Passwörter stimmen nicht überein")]
        public string ConfirmPassword { get; set; } = "";

        [Required, DataType(DataType.Date)]
        public DateTime Birthday { get; set; }
    }
}
