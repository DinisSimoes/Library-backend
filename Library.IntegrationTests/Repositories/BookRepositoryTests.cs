using Library.Domain.Entities;
using Library.Domain.Interfaces;
using Library.Infrastructure.Data;
using Library.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testcontainers.PostgreSql;

namespace Library.IntegrationTests.Repositories
{
    public class BookRepositoryTests : IAsyncLifetime
    {
        private readonly PostgreSqlContainer _pgContainer;
        private LibraryDbContext _context = null!;
        private BookRepository _repository = null!;

        public BookRepositoryTests()
        {
            _pgContainer = new PostgreSqlBuilder()
                .WithDatabase("librarydb")
                .WithUsername("postgres")
                .WithPassword("postgres")
                .Build();
        }

        public async Task InitializeAsync()
        {
            await _pgContainer.StartAsync();

            var options = new DbContextOptionsBuilder<LibraryDbContext>()
                .UseNpgsql(_pgContainer.GetConnectionString())
                .Options;

            _context = new LibraryDbContext(options);
            await _context.Database.EnsureCreatedAsync();

            _repository = new BookRepository(_context);
        }

        public async Task DisposeAsync()
        {
            await _pgContainer.DisposeAsync();
        }

        public async Task<Book> CreateAndAddBookAsync()
        {
            var author = new Author { Id = Guid.NewGuid(), Name = "Author 1" };
            var genre = new Genre { Id = Guid.NewGuid(), Name = "Genre 1" };
            var book = new Book
            {
                Id = Guid.NewGuid(),
                Title = "Book 1",
                Author = author,
                Genre = genre
            };

            _context.Authors.Add(author);
            _context.Genres.Add(genre);
            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return book;
        }

        [Fact]
        public async Task AddAsync_ShouldAddBook()
        {
            // Arrange
            var author = new Author { Id = Guid.NewGuid(), Name = "Author Test" };
            var genre = new Genre { Id = Guid.NewGuid(), Name = "Genre Test" };

            _context.Authors.Add(author);
            _context.Genres.Add(genre);
            await _context.SaveChangesAsync();

            var book = new Book
            {
                Id = Guid.NewGuid(),
                Title = "Book Test",
                AuthorId = author.Id,
                GenreId = genre.Id
            };

            // Act
            await _repository.AddAsync(book);
            var books = await _repository.GetAllAsync();

            // Assert
            Assert.Single(books);
            Assert.Equal("Book Test", books.First().Title);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnBookWithAuthorAndGenre()
        {
            // Arrange
            var author = new Author { Id = Guid.NewGuid(), Name = "Author Test" };
            var genre = new Genre { Id = Guid.NewGuid(), Name = "Genre Test" };

            _context.Authors.Add(author);
            _context.Genres.Add(genre);
            await _context.SaveChangesAsync();

            var book = new Book
            {
                Id = Guid.NewGuid(),
                Title = "Book Test",
                AuthorId = author.Id,
                GenreId = genre.Id
            };

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            // Act
            var retrievedBook = await _repository.GetByIdAsync(book.Id);

            // Assert
            Assert.NotNull(retrievedBook);
            Assert.Equal("Author Test", retrievedBook!.Author!.Name);
            Assert.Equal("Genre Test", retrievedBook.Genre!.Name);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllBooks()
        {
            await CreateAndAddBookAsync();
            var books = await _repository.GetAllAsync();

            Assert.NotEmpty(books);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCorrectBook()
        {
            var book = await CreateAndAddBookAsync();

            var result = await _repository.GetByIdAsync(book.Id);

            Assert.NotNull(result);
            Assert.Equal(book.Title, result!.Title);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateBook()
        {
            var book = await CreateAndAddBookAsync();

            book.Title = "Updated Title";
            await _repository.UpdateAsync(book);

            var updated = await _context.Books.FindAsync(book.Id);
            Assert.Equal("Updated Title", updated!.Title);
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveBook()
        {
            var book = await CreateAndAddBookAsync();

            await _repository.DeleteAsync(book.Id);
            var deleted = await _context.Books.FindAsync(book.Id);

            Assert.Null(deleted);
        }
    }
}
