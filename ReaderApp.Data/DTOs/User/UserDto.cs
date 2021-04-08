using System;
using System.ComponentModel.DataAnnotations;

namespace ReaderApp.Data.DTOs.User
{
    public class UserDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
