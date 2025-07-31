using Library.Application.Interfaces;
using Library.Application.Services;
using Library.Domain.Entities;
using Library.Domain.Interfaces;
using Moq;

namespace Library.UnitTests.Services
{
    public class BookServiceTests
    {
        private readonly Mock<IBookRepository> _bookRepoMock = new();
        private readonly Mock<IValidationService> _validationServiceMock = new();
        private readonly BookService _bookService;

        public BookServiceTests()
        {
            _bookService = new BookService(_bookRepoMock.Object, _validationServiceMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsBooks()
        {
            // Arrange
            var books = new List<Book> { new Book { Id = Guid.NewGuid(), Title = "Test" } };
            _bookRepoMock.Setup(x => x.GetAllAsync()).ReturnsAsync(books);

            // Act
            var result = await _bookService.GetAllAsync();

            // Assert
            Assert.Equal(books, result);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsBook()
        {
            // Arrange
            var id = Guid.NewGuid();
            var book = new Book { Id = id, Title = "Test" };
            _bookRepoMock.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(book);

            // Act
            var result = await _bookService.GetByIdAsync(id);

            // Assert
            Assert.Equal(book, result);
        }

        [Fact]
        public async Task AddAsync_ValidBook_CallsValidationAndRepository()
        {
            // Arrange
            var book = new Book { AuthorId = Guid.NewGuid(), GenreId = Guid.NewGuid() };

            // Act
            await _bookService.AddAsync(book);

            // Assert
            _validationServiceMock.Verify(v => v.ValidateAuthorExistAsync(book.AuthorId), Times.Once);
            _validationServiceMock.Verify(v => v.ValidateGenreExistAsync(book.GenreId), Times.Once);
            _bookRepoMock.Verify(r => r.AddAsync(book), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_CallsRepository()
        {
            var book = new Book { Id = Guid.NewGuid() };

            await _bookService.UpdateAsync(book);

            _bookRepoMock.Verify(x => x.UpdateAsync(book), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_CallsRepository()
        {
            var id = Guid.NewGuid();

            await _bookService.DeleteAsync(id);

            _bookRepoMock.Verify(x => x.DeleteAsync(id), Times.Once);
        }
    }
}
