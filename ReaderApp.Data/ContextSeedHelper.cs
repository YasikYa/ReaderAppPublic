using Microsoft.EntityFrameworkCore;
using ReaderApp.Data.Domain;
using System;

namespace ReaderApp.Data
{
    static class ContextSeedHelper
    {
        public static void SeedData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(new[]
            {
                new User
                {
                    Id = Guid.NewGuid(),
                    Name = "Test User",
                    Email = "test@mail.com",
                    Password = "12345"
                }
            });
        }
    }
}
