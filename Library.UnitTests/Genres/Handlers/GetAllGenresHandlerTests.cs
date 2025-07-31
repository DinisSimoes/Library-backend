using Library.Application.Genres;
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
    public class GetAllGenresHandlerTests
    {
        private readonly Mock<IGenreService> _genreServiceMock;
        private readonly GetAllGenresHandler _handler;

        public GetAllGenresHandlerTests()
        {
            _genreServiceMock = new Mock<IGenreService>();
            _handler = new GetAllGenresHandler(_genreServiceMock.Object);
        }

        [Fact]
        public async Task Handle_WhenCalled_ReturnsListOfGenreDto()
        {
            // Arrange
            var genres = new List<Genre>
            {
                new Genre { Id = Guid.NewGuid(), Name = "Fiction" },
                new Genre { Id = Guid.NewGuid(), Name = "Non-Fiction" }
            };

            _genreServiceMock.Setup(s => s.GetAllAsync())
                .ReturnsAsync(genres);

            var query = new GetAllGenresQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            var resultList = result.ToList();
            Assert.Equal(genres.Count, resultList.Count);
            Assert.All(resultList, dto => Assert.IsType<GenreDto>(dto));
            Assert.Contains(resultList, dto => dto.Name == "Fiction");
            Assert.Contains(resultList, dto => dto.Name == "Non-Fiction");

            _genreServiceMock.Verify(s => s.GetAllAsync(), Times.Once);
        }
    }
}
