using Library.Application.Genres.Handlers;
using Library.Application.Genres.Queries;
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
    public class GetGenreByIdHandlerTests
    {
        private readonly Mock<IGenreService> _genreServiceMock;
        private readonly GetGenreByIdHandler _handler;

        public GetGenreByIdHandlerTests()
        {
            _genreServiceMock = new Mock<IGenreService>();
            _handler = new GetGenreByIdHandler(_genreServiceMock.Object);
        }

        [Fact]
        public async Task Handle_GenreExists_ReturnsGenreDto()
        {
            // Arrange
            var genreId = Guid.NewGuid();
            var genre = new Genre
            {
                Id = genreId,
                Name = "Science Fiction"
            };

            _genreServiceMock.Setup(s => s.GetByIdAsync(genreId))
                             .ReturnsAsync(genre);

            var query = new GetGenreByIdQuery(genreId);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(genreId, result.Id);
            Assert.Equal("Science Fiction", result.Name);

            _genreServiceMock.Verify(s => s.GetByIdAsync(genreId), Times.Once);
        }

        [Fact]
        public async Task Handle_GenreDoesNotExist_ReturnsNull()
        {
            // Arrange
            var genreId = Guid.NewGuid();

            _genreServiceMock.Setup(s => s.GetByIdAsync(genreId))
                             .ReturnsAsync((Genre?)null);

            var query = new GetGenreByIdQuery(genreId);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Null(result);
            _genreServiceMock.Verify(s => s.GetByIdAsync(genreId), Times.Once);
        }
    }
}
