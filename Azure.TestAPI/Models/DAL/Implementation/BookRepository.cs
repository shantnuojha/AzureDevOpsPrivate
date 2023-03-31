using Azure.TestAPI.Models.DAL.Contract;
using Azure.TestAPI.Models.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Azure.TestAPI.Models.DAL.Implementation
{
    public class BookRepository : IBookRepo
    {
        public readonly BookDBContext _bookDBContext; 
        public BookRepository(BookDBContext bookDBContext)
        {
            _bookDBContext = bookDBContext;
        }
        public async Task<IActionResult> DeleteBook(int Id)
        {
            var book = await _bookDBContext.Books.FindAsync(Id);
            if(book == null)
            {
                return new NotFoundResult();
            }
            _bookDBContext.Books.Remove(book);
            await _bookDBContext.SaveChangesAsync();
            return new NoContentResult();
        }

        public async Task<ActionResult<Book>> GetBook(int Id)
        {
            var book = await _bookDBContext.Books.FindAsync(Id);
            if (book == null)
            {
                return new NotFoundResult();
            }
            return book;
        }

        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            return await _bookDBContext.Books.ToListAsync();
        }

        public async Task<ActionResult<Book>> PostBook(Book book)
        {
            _bookDBContext.Books.Add(book);
            await _bookDBContext.SaveChangesAsync();
            return book;
        }

        public async Task<IActionResult> PutBook(int Id, Book book)
        {
            if(Id!=book.Id)
            {
                return new BadRequestResult();
            }

            _bookDBContext.Entry(book).State = EntityState.Modified;

            try
            {
                await _bookDBContext.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException)
            {
                if(!BookExists(Id))
                {
                    return new NotFoundResult();
                }
                else
                {
                    throw;
                }
            }

            return new NoContentResult();
        }
        private bool BookExists(int Id)
        {
            return _bookDBContext.Books.Any(b => b.Id == Id);
        }
    }
}
