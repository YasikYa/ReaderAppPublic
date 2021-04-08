using Microsoft.EntityFrameworkCore;
using ReaderApp.Data.Domain;

namespace ReaderApp.Data
{
    public class ReaderAppContext : DbContext
    {
        public ReaderAppContext(DbContextOptions<ReaderAppContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        public DbSet<TextFile> TextFiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            ConfigureRelations(modelBuilder);
            ContextSeedHelper.SeedData(modelBuilder);
        }

        private void ConfigureRelations(ModelBuilder builder)
        {
            builder.Entity<User>()
                   .HasMany(u => u.TextFiles)
                   .WithOne(tf => tf.User)
                   .HasForeignKey(tf => tf.UserId)
                   .OnDelete(DeleteBehavior.Restrict); // All user related files must be cleaned from physical storage before romoved from DB.
        }
    }
}
