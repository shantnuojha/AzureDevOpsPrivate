using Azure.TestAPI.Models.Entity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Azure.TestAPI.Models.DAL.Contract
{
    public interface IBookRepo
    {
        Task<ActionResult<IEnumerable<Book>>> GetBooks();
        Task<ActionResult<Book>> GetBook(int Id);
        Task<IActionResult> PutBook(int Id, Book book);
        Task<ActionResult<Book>> PostBook(Book book);
        Task<IActionResult> DeleteBook(int Id);
    }
}
