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
    public class ValidationServiceTests
    {
        private readonly Mock<IAuthorRepository> _authorRepositoryMock;
        private readonly Mock<IGenreRepository> _genreRepositoryMock;
        private readonly ValidationService _validationService;

        public ValidationServiceTests()
        {
            _authorRepositoryMock = new Mock<IAuthorRepository>();
            _genreRepositoryMock = new Mock<IGenreRepository>();
            _validationService = new ValidationService(_authorRepositoryMock.Object, _genreRepositoryMock.Object);
        }

        [Fact]
        public async Task ValidateAuthorExistAsync_AuthorExists_DoesNotThrow()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            _authorRepositoryMock.Setup(r => r.GetByIdAsync(authorId))
                                 .ReturnsAsync(new Author { Id = authorId });

            // Act & Assert
            await _validationService.ValidateAuthorExistAsync(authorId);
        }

        [Fact]
        public async Task ValidateAuthorExistAsync_AuthorDoesNotExist_ThrowsNotFoundException()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            _authorRepositoryMock.Setup(r => r.GetByIdAsync(authorId))
                                 .ReturnsAsync((Author?)null);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<NotFoundException>(() => _validationService.ValidateAuthorExistAsync(authorId));
            Assert.Contains("Author", ex.Message);
        }

        [Fact]
        public async Task ValidateGenreExistAsync_GenreExists_DoesNotThrow()
        {
            // Arrange
            var genreId = Guid.NewGuid();
            _genreRepositoryMock.Setup(r => r.GetByIdAsync(genreId))
                                .ReturnsAsync(new Domain.Entities.Genre { Id = genreId });

            // Act & Assert
            await _validationService.ValidateGenreExistAsync(genreId);
        }

        [Fact]
        public async Task ValidateGenreExistAsync_GenreDoesNotExist_ThrowsNotFoundException()
        {
            // Arrange
            var genreId = Guid.NewGuid();
            _genreRepositoryMock.Setup(r => r.GetByIdAsync(genreId))
                                .ReturnsAsync((Domain.Entities.Genre?)null);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<NotFoundException>(() => _validationService.ValidateGenreExistAsync(genreId));
            Assert.Contains("Genre", ex.Message);
        }
    }
}
