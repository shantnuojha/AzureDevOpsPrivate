using Azure.TestAPI.Models.DAL.Contract;
using Azure.TestAPI.Models.Entity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Azure.TestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepo _bookRepo;
        public BooksController(IBookRepo bookRepo)
        {
            _bookRepo = bookRepo;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            try
            {
                return await _bookRepo.GetBooks();
            }
            catch
            {
                return StatusCode(500);
            }
        }
        [HttpGet("{Id}")]
        public async Task<ActionResult<Book>> GetBook(int Id)
        {
            try
            {
                return await _bookRepo.GetBook(Id);
            }
            catch
            {
                return StatusCode(500);
            }
        }
        [HttpPut("{Id}")]
        public async Task<IActionResult> PutBook(int Id, Book book)
        {
            try
            {
                return await _bookRepo.PutBook(Id, book);
            }
            catch(DbUpdateConcurrencyException)
            {
                return StatusCode(409);
            }
            catch
            {
                return StatusCode(500);
            }
        }
        [HttpPost]
        public async Task<ActionResult<Book>> PostBook(Book book)
        {
            try
            {
                var result = await _bookRepo.PostBook(book);
                return CreatedAtAction("GetBook", new { Id = result.Value.Id }, book);
            }
            catch
            {
                return StatusCode(500);
            }
        }
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteBook(int Id, Book book)
        {
            try
            {
                return await _bookRepo.DeleteBook(Id);
            }
            catch
            {
                return StatusCode(500);
            }
        }
    }
}
