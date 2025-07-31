using Library.Application.Books.Handlers;
using Library.Application.Books.Queries;
using Library.Application.Interfaces;
using Library.Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.UnitTests.Books.Handlers
{
    public class GetBookByIdHandlerTests
    {
        private readonly Mock<IBookService> _bookServiceMock;
        private readonly GetBookByIdHandler _handler;

        public GetBookByIdHandlerTests()
        {
            _bookServiceMock = new Mock<IBookService>();
            _handler = new GetBookByIdHandler(_bookServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnBookDto_WhenBookExists()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            var book = new Book
            {
                Id = bookId,
                Title = "Clean Code",
                Author = new Author { Name = "Robert C. Martin" },
                Genre = new Genre { Name = "Software Engineering" }
            };

            _bookServiceMock
                .Setup(s => s.GetByIdAsync(bookId))
                .ReturnsAsync(book);

            var query = new GetBookByIdQuery(bookId);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(bookId, result!.Id);
            Assert.Equal("Clean Code", result.Title);
            Assert.Equal("Robert C. Martin", result.AuthorName);
            Assert.Equal("Software Engineering", result.GenreName);
        }

        [Fact]
        public async Task Handle_ShouldReturnNull_WhenBookDoesNotExist()
        {
            // Arrange
            var bookId = Guid.NewGuid();

            _bookServiceMock
                .Setup(s => s.GetByIdAsync(bookId))
                .ReturnsAsync((Book?)null);

            var query = new GetBookByIdQuery(bookId);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Null(result);
        }
    }
}
