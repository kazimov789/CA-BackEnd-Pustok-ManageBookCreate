using Microsoft.EntityFrameworkCore;
using P328Pustok.Models;

namespace P328Pustok.DAL
{
    public class PustokContext:DbContext
    {
        public PustokContext(DbContextOptions<PustokContext> options):base(options)
        {

        }

        public DbSet<Tag> Tags { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Author> Authors { get; set; }  
        public DbSet<Book> Books { get; set; }
        public DbSet<BookImage> BookImages { get; set; }
        public DbSet<Images> Images { get; set; }

        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Setting> Settings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<BookTag>().HasKey(x => new { x.TagId, x.BookId });
            modelBuilder.Entity<Setting>().HasKey(x => x.Key);

            base.OnModelCreating(modelBuilder);
        }
    }
}
