using BookAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookAPI.Data
{
    public class DataContext : DbContext
    {
        private DbSet<Book> books;

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
                
        }

        public DbSet<Book> Books { get; set; }
    }
}
