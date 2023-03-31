using Azure.TestAPI.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace Azure.TestAPI.Models
{
    public class BookDBContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public BookDBContext(DbContextOptions<BookDBContext> options) : base(options)
        {

        }
    }
}
