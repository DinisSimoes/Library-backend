using Library.Application.Genres.Commands;
using Library.Application.Genres.Handlers;
using Library.Application.Interfaces;
using Library.Domain.Entities;
using MediatR;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.UnitTests.Genres.Handlers
{
    public class UpdateGenreHandlerTests
    {
        private readonly Mock<IGenreService> _genreServiceMock;
        private readonly UpdateGenreHandler _handler;

        public UpdateGenreHandlerTests()
        {
            _genreServiceMock = new Mock<IGenreService>();
            _handler = new UpdateGenreHandler(_genreServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ExistingGenre_UpdatesGenreAndReturnsUnit()
        {
            // Arrange
            var genreId = Guid.NewGuid();
            var existingGenre = new Genre
            {
                Id = genreId,
                Name = "Old Name"
            };

            var command = new UpdateGenreCommand(genreId, "New Name");

            _genreServiceMock.Setup(s => s.GetByIdAsync(genreId))
                             .ReturnsAsync(existingGenre);

            _genreServiceMock.Setup(s => s.UpdateAsync(It.IsAny<Genre>()))
                             .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(Unit.Value, result);
            Assert.Equal("New Name", existingGenre.Name);
            _genreServiceMock.Verify(s => s.GetByIdAsync(genreId), Times.Once);
            _genreServiceMock.Verify(s => s.UpdateAsync(existingGenre), Times.Once);
        }

        [Fact]
        public async Task Handle_NonExistingGenre_ThrowsKeyNotFoundException()
        {
            // Arrange
            var genreId = Guid.NewGuid();
            var command = new UpdateGenreCommand(genreId, "New Name");

            _genreServiceMock.Setup(s => s.GetByIdAsync(genreId))
                             .ReturnsAsync((Genre?)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(command, CancellationToken.None));

            _genreServiceMock.Verify(s => s.GetByIdAsync(genreId), Times.Once);
            _genreServiceMock.Verify(s => s.UpdateAsync(It.IsAny<Genre>()), Times.Never);
        }
    }
}
