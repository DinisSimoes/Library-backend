using Library.Application.Books.Commands;
using Library.Application.Books.Handlers;
using Library.Application.Interfaces;
using MediatR;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Library.UnitTests.Books.Handlers
{
    public class DeleteBookHandlerTests
    {
        private readonly Mock<IBookService> _bookServiceMock;
        private readonly DeleteBookHandler _handler;

        public DeleteBookHandlerTests()
        {
            _bookServiceMock = new Mock<IBookService>();
            _handler = new DeleteBookHandler(_bookServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ValidCommand_DeletesBookAndReturnsUnit()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            var command = new DeleteBookCommand(bookId);

            _bookServiceMock
                .Setup(s => s.DeleteAsync(bookId))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            _bookServiceMock.Verify(s => s.DeleteAsync(bookId), Times.Once);
            Assert.Equal(Unit.Value, result);
        }

        [Fact]
        public async Task Handle_DeleteThrowsException_PropagatesException()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            var command = new DeleteBookCommand(bookId);

            _bookServiceMock
                .Setup(s => s.DeleteAsync(bookId))
                .ThrowsAsync(new Exception("Erro ao deletar"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() =>
                _handler.Handle(command, CancellationToken.None));

            Assert.Equal("Erro ao deletar", exception.Message);
            _bookServiceMock.Verify(s => s.DeleteAsync(bookId), Times.Once);
        }
    }
}
