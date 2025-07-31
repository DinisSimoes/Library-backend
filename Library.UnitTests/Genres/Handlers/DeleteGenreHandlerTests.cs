using Library.Application.Genres.Commands;
using Library.Application.Genres.Handlers;
using Library.Application.Interfaces;
using MediatR;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.UnitTests.Genres.Handlers
{
    public class DeleteGenreHandlerTests
    {
        private readonly Mock<IGenreService> _genreServiceMock;
        private readonly DeleteGenreHandler _handler;

        public DeleteGenreHandlerTests()
        {
            _genreServiceMock = new Mock<IGenreService>();
            _handler = new DeleteGenreHandler(_genreServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ValidId_CallsDeleteAsync_ReturnsUnit()
        {
            // Arrange
            var genreId = Guid.NewGuid();
            _genreServiceMock.Setup(s => s.DeleteAsync(genreId))
                .Returns(Task.CompletedTask);

            var command = new DeleteGenreCommand(genreId);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            _genreServiceMock.Verify(s => s.DeleteAsync(genreId), Times.Once);
            Assert.Equal(Unit.Value, result);
        }
    }
}
