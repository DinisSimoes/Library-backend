using Library.Application.Books.Commands;
using Library.Application.Books.Handlers;
using Library.Application.Interfaces;
using Library.Domain.Entities;
using MediatR;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.UnitTests.Books.Handlers
{
    public class UpdateBookHandlerTests
    {
        private readonly Mock<IBookService> _bookServiceMock;
        private readonly UpdateBookHandler _handler;

        public UpdateBookHandlerTests()
        {
            _bookServiceMock = new Mock<IBookService>();
            _handler = new UpdateBookHandler(_bookServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ValidCommand_CallsUpdateAndReturnsUnit()
        {
            // Arrange
            var command = new UpdateBookCommand(
                Guid.NewGuid(),
                "Refactoring",
                Guid.NewGuid(),
                Guid.NewGuid()
            );

            Book? capturedBook = null;
            _bookServiceMock
                .Setup(s => s.UpdateAsync(It.IsAny<Book>()))
                .Callback<Book>(b => capturedBook = b)
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            _bookServiceMock.Verify(s => s.UpdateAsync(It.IsAny<Book>()), Times.Once);
            Assert.Equal(Unit.Value, result);
            Assert.NotNull(capturedBook);
            Assert.Equal(command.Id, capturedBook!.Id);
            Assert.Equal(command.Title, capturedBook.Title);
            Assert.Equal(command.AuthorId, capturedBook.AuthorId);
            Assert.Equal(command.GenreId, capturedBook.GenreId);
        }
    }
}
