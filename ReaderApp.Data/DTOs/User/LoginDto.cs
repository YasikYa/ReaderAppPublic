using System.ComponentModel.DataAnnotations;

namespace ReaderApp.Data.DTOs.User
{
    public class LoginDto
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
