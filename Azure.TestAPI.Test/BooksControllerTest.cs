using Moq;
using Xunit;
using System;
using System.Threading;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Azure.TestAPI.Controllers;
using Azure.TestAPI.Models;
using Azure.TestAPI.Models.Entity;
using Azure.TestAPI.Models.DAL.Contract;
using System.Threading.Tasks;

namespace Azure.TestAPI.Test
{
    public class BooksControllerTest
    {
        private BooksController _booksController;
        private int Id = 1;
        private readonly Mock<IBookRepo> _bookStub = new Mock<IBookRepo>();
        Book newSamplebook = new Book
        {
            Id = 1,
            Name = "State Patsy",
            Genere = "Action/Adventure",
            PublisherName = "Queens"
        };
        Book toBePostedBook = new Book
        {
            Name = "Fedral Matters",
            Genere = "Suspense",
            PublisherName = "Harpers"
        };
        [Fact]
        public async Task GetBook_BasedOnId_WithoutExistingBook_ReturnNotFound()
        {
            _booksController = new BooksController(_bookStub.Object);
            _bookStub.Setup(repo => repo.GetBook(It.IsAny<int>())).ReturnsAsync(new NotFoundResult());
            var actionResult = await _booksController.GetBook(1);
            Assert.IsType<NotFoundResult>(actionResult.Result);
        }
        [Fact]
        public async Task GetBook_BasedOnId_WithExstingBook_ReturnBook()
        {
            _bookStub.Setup(repo => repo.GetBook(It.IsAny<int>())).ReturnsAsync(newSamplebook);
            _booksController = new BooksController(_bookStub.Object);
            var actionResult = await _booksController.GetBook(1);
            Assert.IsType<Book>(actionResult.Value);
            var result = actionResult.Value;
            newSamplebook.Should().BeEquivalentTo(result, options => options.ComparingByMembers<Book>());
        }
        [Fact]
        public async Task PostBook_WithNewBook_ReturnNewlyCreatedBook()
        {
            _bookStub.Setup(repo => repo.PostBook(It.IsAny<Book>())).ReturnsAsync(newSamplebook);
            _booksController = new BooksController(_bookStub.Object);
            var actionResult = await _booksController.PostBook(toBePostedBook);
            Assert.Equal("201", ((CreatedAtActionResult)actionResult.Result).StatusCode.ToString());
        }
        [Fact]
        public async Task PostBook_WithException_ReturnsInternalServerError()
        {
            _bookStub.Setup(repo => repo.PostBook(It.IsAny<Book>())).Throws(new Exception());
            _booksController = new BooksController(_bookStub.Object);
            var actionResult = await _booksController.PostBook(null);
            Assert.Equal("500", ((StatusCodeResult)actionResult.Result).StatusCode.ToString());
        }
        [Fact]
        public async Task PutBook_WithException_ReturnsConcurrencyException()
        {
            _bookStub.Setup(repo => repo.PutBook(It.IsAny<int>(), It.IsAny<Book>())).Throws(new DbUpdateConcurrencyException());
            _booksController = new BooksController(_bookStub.Object);
            var actionResult = await _booksController.PutBook(Id, newSamplebook);
            Assert.Equal("409", ((StatusCodeResult)actionResult).StatusCode.ToString());
        }
        [Fact]
        public async Task PutBook_WithException_ReturnsException()
        {
            _bookStub.Setup(repo => repo.PutBook(It.IsAny<int>(), It.IsAny<Book>())).Throws(new Exception());
            _booksController = new BooksController(_bookStub.Object);
            var actionResult = await _booksController.PutBook(Id, newSamplebook);
            Assert.Equal("500", ((StatusCodeResult)actionResult).StatusCode.ToString());
        }
        [Fact]
        public async Task PutBook_WithExistingBook_basedOnId_ReturnUpdatedBook()
        {

            _bookStub.Setup(repo => repo.PutBook(It.IsAny<int>(), It.IsAny<Book>())).ReturnsAsync(new NoContentResult());
            _booksController = new BooksController(_bookStub.Object);
            var actionResult = await _booksController.PutBook(Id, newSamplebook);
            actionResult.Should().BeOfType<NoContentResult>();
        }
    }
}
