using System;

namespace ReaderApp.Data.Domain
{
    public class TextFile
    {
        public Guid Id { get; set; }

        public string FileName { get; set; }

        public Guid UserId { get; set; }

        public User User { get; set; }
    }
}
