using System;
using System.ComponentModel.DataAnnotations;

namespace ReaderApp.Data.DTOs.File
{
    public class FileDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string FileName { get; set; }
    }
}
