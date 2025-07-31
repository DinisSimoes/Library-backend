using Library.Domain.Entities;
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
    public class AuthorRepositoryTests : IAsyncLifetime
    {
        private readonly PostgreSqlContainer _pgContainer;
        private LibraryDbContext _context = null!;
        private AuthorRepository _repository = null!;

        public AuthorRepositoryTests()
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

            _repository = new AuthorRepository(_context);
        }

        public async Task DisposeAsync()
        {
            await _pgContainer.DisposeAsync();
        }

        [Fact]
        public async Task AddAsync_ShouldAddAuthor()
        {
            var author = new Author
            {
                Id = Guid.NewGuid(),
                Name = "George Orwell",
                Books = new List<Domain.Entities.Book>()
            };

            await _repository.AddAsync(author);
            var found = await _repository.GetByIdAsync(author.Id);

            Assert.NotNull(found);
            Assert.Equal("George Orwell", found!.Name);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllAuthors()
        {
            var author1 = new Author { Id = Guid.NewGuid(), Name = "Author 1" };
            var author2 = new Author { Id = Guid.NewGuid(), Name = "Author 2" };

            await _repository.AddAsync(author1);
            await _repository.AddAsync(author2);

            var authors = await _repository.GetAllAsync();

            Assert.Equal(2, authors.Count());
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateAuthor()
        {
            var author = new Author { Id = Guid.NewGuid(), Name = "Original" };
            await _repository.AddAsync(author);

            author.Name = "Updated";
            await _repository.UpdateAsync(author);

            var updated = await _repository.GetByIdAsync(author.Id);

            Assert.NotNull(updated);
            Assert.Equal("Updated", updated!.Name);
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveAuthor()
        {
            var author = new Author { Id = Guid.NewGuid(), Name = "ToDelete" };
            await _repository.AddAsync(author);

            await _repository.DeleteAsync(author.Id);
            var result = await _repository.GetByIdAsync(author.Id);

            Assert.Null(result);
        }
    }
}
