using System;
using System.Collections.Generic;

namespace ReaderApp.Data.Domain
{
    public class User
    {
        public User()
        {
            TextFiles = new HashSet<TextFile>();
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public ICollection<TextFile> TextFiles { get; set; }
    }
}
