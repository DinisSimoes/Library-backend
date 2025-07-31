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
    public class GetAllBooksHandlerTests
    {
        private readonly Mock<IBookService> _bookServiceMock;
        private readonly GetAllBooksHandler _handler;

        public GetAllBooksHandlerTests()
        {
            _bookServiceMock = new Mock<IBookService>();
            _handler = new GetAllBooksHandler(_bookServiceMock.Object);
        }

        [Fact]
        public async Task Handle_WhenBooksExist_ReturnsMappedBookDtos()
        {
            // Arrange
            var books = new List<Book>
            {
                new Book
                {
                    Id = Guid.NewGuid(),
                    Title = "Clean Code",
                    Author = new Author { Name = "Robert C. Martin" },
                    Genre = new Genre { Name = "Programming" }
                },
                new Book
                {
                    Id = Guid.NewGuid(),
                    Title = "The Pragmatic Programmer",
                    Author = new Author { Name = "Andy Hunt" },
                    Genre = new Genre { Name = "Programming" }
                }
            };

            _bookServiceMock.Setup(s => s.GetAllAsync())
                .ReturnsAsync(books);

            var query = new GetAllBooksQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            var resultList = result.ToList();
            Assert.Equal(2, resultList.Count);
            Assert.Equal("Clean Code", resultList[0].Title);
            Assert.Equal("Robert C. Martin", resultList[0].AuthorName);
            Assert.Equal("Programming", resultList[0].GenreName);
            Assert.Equal("The Pragmatic Programmer", resultList[1].Title);
            Assert.Equal("Andy Hunt", resultList[1].AuthorName);
        }

        [Fact]
        public async Task Handle_WhenNoBooksExist_ReturnsEmptyList()
        {
            // Arrange
            _bookServiceMock.Setup(s => s.GetAllAsync())
                .ReturnsAsync(new List<Book>());

            var query = new GetAllBooksQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

    }
}
