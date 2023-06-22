using BookLibrary.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookLibrary.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {


        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Book>()
                .HasQueryFilter(b => !b.Deleted);
            builder.Entity<Author>()
                .HasQueryFilter(b => !b.Deleted);
            builder.Entity<Publisher>()
                .HasQueryFilter(b => !b.Deleted);

            base.OnModelCreating(builder);
        }
        public virtual DbSet<Book> Books { get; set; }

        public virtual DbSet<Author> Authors { get; set; }

        public virtual DbSet<Publisher> Publishers { get; set; }

    }
}