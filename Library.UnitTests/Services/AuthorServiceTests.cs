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
    public class AuthorServiceTests
    {
        private readonly Mock<IAuthorRepository> _authorRepositoryMock;
        private readonly AuthorService _authorService;

        public AuthorServiceTests()
        {
            _authorRepositoryMock = new Mock<IAuthorRepository>();
            _authorService = new AuthorService(_authorRepositoryMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAuthors()
        {
            // Arrange
            var authors = new List<Author> { new() { Id = Guid.NewGuid(), Name = "Author 1" } };
            _authorRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(authors);

            // Act
            var result = await _authorService.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
        }

        [Fact]
        public async Task GetByIdAsync_ExistingId_ReturnsAuthor()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            var author = new Author { Id = authorId, Name = "Author Test" };
            _authorRepositoryMock.Setup(r => r.GetByIdAsync(authorId)).ReturnsAsync(author);

            // Act
            var result = await _authorService.GetByIdAsync(authorId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(authorId, result.Id);
        }

        [Fact]
        public async Task GetByIdAsync_NonExistingId_ThrowsNotFoundException()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            _authorRepositoryMock.Setup(r => r.GetByIdAsync(authorId)).ReturnsAsync((Author?)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _authorService.GetByIdAsync(authorId));
        }

        [Fact]
        public async Task AddAsync_CallsRepository()
        {
            // Arrange
            var author = new Author { Id = Guid.NewGuid(), Name = "New Author" };

            // Act
            await _authorService.AddAsync(author);

            // Assert
            _authorRepositoryMock.Verify(r => r.AddAsync(author), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ExistingAuthor_CallsRepository()
        {
            // Arrange
            var author = new Author { Id = Guid.NewGuid(), Name = "Updated Author" };
            _authorRepositoryMock.Setup(r => r.GetByIdAsync(author.Id)).ReturnsAsync(author);

            // Act
            await _authorService.UpdateAsync(author);

            // Assert
            _authorRepositoryMock.Verify(r => r.UpdateAsync(author), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_NonExistingAuthor_ThrowsNotFoundException()
        {
            // Arrange
            var author = new Author { Id = Guid.NewGuid(), Name = "Non-existing" };
            _authorRepositoryMock.Setup(r => r.GetByIdAsync(author.Id)).ReturnsAsync((Author?)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _authorService.UpdateAsync(author));
        }

        [Fact]
        public async Task DeleteAsync_ExistingAuthorWithoutBooks_CallsDelete()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            var author = new Author { Id = authorId, Books = new List<Book>() };
            _authorRepositoryMock.Setup(r => r.GetByIdAsync(authorId)).ReturnsAsync(author);

            // Act
            await _authorService.DeleteAsync(authorId);

            // Assert
            _authorRepositoryMock.Verify(r => r.DeleteAsync(authorId), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ExistingAuthorWithBooks_ThrowsArgumentException()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            var author = new Author
            {
                Id = authorId,
                Books = new List<Book> { new() { Id = Guid.NewGuid(), Title = "Book 1" } }
            };
            _authorRepositoryMock.Setup(r => r.GetByIdAsync(authorId)).ReturnsAsync(author);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(() => _authorService.DeleteAsync(authorId));
            Assert.Equal("Não é possível deletar o autor pois há livros associados.", ex.Message);
        }

        [Fact]
        public async Task DeleteAsync_NonExistingAuthor_ThrowsNotFoundException()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            _authorRepositoryMock.Setup(r => r.GetByIdAsync(authorId)).ReturnsAsync((Author?)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _authorService.DeleteAsync(authorId));
        }
    }
}
