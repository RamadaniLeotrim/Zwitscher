using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zwitscher.Models
{
    public class ChatMessage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Text { get; set; } = "";

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Fremdschlüssel auf den Author
        [Required]
        public string UserId { get; set; } = "";

        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }
    }
}
