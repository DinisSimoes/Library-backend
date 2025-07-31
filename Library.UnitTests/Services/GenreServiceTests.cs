using Library.Application.Exceptions;
using Library.Application.Services;
using Library.Domain.Entities;
using Library.Domain.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.UnitTests.Services
{
    public class GenreServiceTests
    {
        private readonly Mock<IGenreRepository> _genreRepositoryMock;
        private readonly GenreService _genreService;

        public GenreServiceTests()
        {
            _genreRepositoryMock = new Mock<IGenreRepository>();
            _genreService = new GenreService(_genreRepositoryMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnGenres()
        {
            // Arrange
            var genres = new List<Genre>
            {
                new Genre { Id = Guid.NewGuid(), Name = "Fiction" },
                new Genre { Id = Guid.NewGuid(), Name = "Romance" }
            };

            _genreRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(genres);

            // Act
            var result = await _genreService.GetAllAsync();

            // Assert
            Assert.Equal(genres.Count, result.Count());
            _genreRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnGenre_WhenFound()
        {
            // Arrange
            var genreId = Guid.NewGuid();
            var genre = new Genre { Id = genreId, Name = "Sci-Fi" };

            _genreRepositoryMock.Setup(repo => repo.GetByIdAsync(genreId)).ReturnsAsync(genre);

            // Act
            var result = await _genreService.GetByIdAsync(genreId);

            // Assert
            Assert.Equal(genre, result);
            _genreRepositoryMock.Verify(repo => repo.GetByIdAsync(genreId), Times.Once);
        }

        [Fact]
        public async Task AddAsync_ShouldCallRepository()
        {
            // Arrange
            var genre = new Genre { Id = Guid.NewGuid(), Name = "Adventure" };

            // Act
            await _genreService.AddAsync(genre);

            // Assert
            _genreRepositoryMock.Verify(repo => repo.AddAsync(genre), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldCallRepository()
        {
            // Arrange
            var genre = new Genre { Id = Guid.NewGuid(), Name = "Biography" };

            // Act
            await _genreService.UpdateAsync(genre);

            // Assert
            _genreRepositoryMock.Verify(repo => repo.UpdateAsync(genre), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteGenre_WhenValid()
        {
            // Arrange
            var genreId = Guid.NewGuid();
            var genre = new Genre { Id = genreId, Name = "Drama", Books = new List<Book>() };

            _genreRepositoryMock.Setup(repo => repo.GetByIdAsync(genreId)).ReturnsAsync(genre);

            // Act
            await _genreService.DeleteAsync(genreId);

            // Assert
            _genreRepositoryMock.Verify(repo => repo.DeleteAsync(genreId), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowNotFoundException_WhenGenreNotFound()
        {
            // Arrange
            var genreId = Guid.NewGuid();

            _genreRepositoryMock.Setup(repo => repo.GetByIdAsync(genreId)).ReturnsAsync((Genre)null!);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _genreService.DeleteAsync(genreId));
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowArgumentException_WhenGenreHasBooks()
        {
            // Arrange
            var genreId = Guid.NewGuid();
            var genre = new Genre
            {
                Id = genreId,
                Name = "History",
                Books = new List<Book> { new Book { Id = Guid.NewGuid(), Title = "Test Book" } }
            };

            _genreRepositoryMock.Setup(repo => repo.GetByIdAsync(genreId)).ReturnsAsync(genre);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(() => _genreService.DeleteAsync(genreId));
            Assert.Equal("Não é possível deletar o gênero pois há livros associados.", ex.Message);
        }
    }
}
