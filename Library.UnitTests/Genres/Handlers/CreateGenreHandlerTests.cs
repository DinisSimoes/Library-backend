using Library.Application.Genres.Commands;
using Library.Application.Genres.Handlers;
using Library.Application.Interfaces;
using Library.Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.UnitTests.Genres.Handlers
{
    public class CreateGenreHandlerTests
    {
        private readonly Mock<IGenreService> _genreServiceMock;
        private readonly CreateGenreHandler _handler;

        public CreateGenreHandlerTests()
        {
            _genreServiceMock = new Mock<IGenreService>();
            _handler = new CreateGenreHandler(_genreServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ValidCommand_CallsAddAsyncAndReturnsGuid()
        {
            // Arrange
            var command = new CreateGenreCommand("Science Fiction");

            Genre? capturedGenre = null;
            _genreServiceMock
                .Setup(s => s.AddAsync(It.IsAny<Genre>()))
                .Callback<Genre>(g => capturedGenre = g)
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            _genreServiceMock.Verify(s => s.AddAsync(It.IsAny<Genre>()), Times.Once);
            Assert.NotEqual(Guid.Empty, result);
            Assert.NotNull(capturedGenre);
            Assert.Equal(result, capturedGenre!.Id);
            Assert.Equal(command.Name, capturedGenre.Name);
        }
    }
}
