using Library.Application.Authors.Commands;
using Library.Application.Authors.Handlers;
using Library.Domain.Entities;
using Library.Domain.Interfaces;
using MediatR;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.UnitTests.Authors.Handlers
{
    public class UpdateAuthorHandlerTests
    {
        private readonly Mock<IAuthorRepository> _repositoryMock;
        private readonly UpdateAuthorHandler _handler;

        public UpdateAuthorHandlerTests()
        {
            _repositoryMock = new Mock<IAuthorRepository>();
            _handler = new UpdateAuthorHandler(_repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_AuthorExists_UpdatesAuthorAndReturnsUnit()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            var existingAuthor = new Author { Id = authorId, Name = "Old Name" };
            var command = new UpdateAuthorCommand(authorId, "New Name");

            _repositoryMock.Setup(r => r.GetByIdAsync(authorId))
                           .ReturnsAsync(existingAuthor);
            _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Author>()))
                           .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            _repositoryMock.Verify(r => r.GetByIdAsync(authorId), Times.Once);
            _repositoryMock.Verify(r => r.UpdateAsync(It.Is<Author>(a => a.Id == authorId && a.Name == "New Name")), Times.Once);
            Assert.Equal(Unit.Value, result);
        }

        [Fact]
        public async Task Handle_AuthorDoesNotExist_ThrowsKeyNotFoundException()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            var command = new UpdateAuthorCommand(authorId, "New Name");

            _repositoryMock.Setup(r => r.GetByIdAsync(authorId))
                           .ReturnsAsync((Author?)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));

            Assert.Equal("Autor não encontrado.", exception.Message);
            _repositoryMock.Verify(r => r.GetByIdAsync(authorId), Times.Once);
            _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Author>()), Times.Never);
        }
    }
}
