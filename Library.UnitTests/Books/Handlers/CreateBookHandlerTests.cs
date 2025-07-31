using Library.Application.Books.Commands;
using Library.Application.Books.Handlers;
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
    public class CreateBookHandlerTests
    {
        private readonly Mock<IBookService> _bookServiceMock;
        private readonly CreateBookHandler _handler;

        public CreateBookHandlerTests()
        {
            _bookServiceMock = new Mock<IBookService>();
            _handler = new CreateBookHandler(_bookServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ValidCommand_AddsBookAndReturnsId()
        {
            // Arrange
            var command = new CreateBookCommand(
                "Clean Code",
                Guid.NewGuid(),
                Guid.NewGuid()
            );

            Book capturedBook = null!;
            _bookServiceMock
                .Setup(s => s.AddAsync(It.IsAny<Book>()))
                .Callback<Book>(book => capturedBook = book)
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            _bookServiceMock.Verify(s => s.AddAsync(It.IsAny<Book>()), Times.Once);
            Assert.NotEqual(Guid.Empty, result);
            Assert.Equal(result, capturedBook.Id);
            Assert.Equal(command.Title, capturedBook.Title);
            Assert.Equal(command.AuthorId, capturedBook.AuthorId);
            Assert.Equal(command.GenreId, capturedBook.GenreId);
        }
    }
}
