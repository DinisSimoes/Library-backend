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
    public class GenreRepositoryTests : IAsyncLifetime
    {
        private readonly PostgreSqlContainer _pgContainer;
        private LibraryDbContext _dbContext = null!;
        private GenreRepository _genreRepository = null!;

        public GenreRepositoryTests()
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

            _dbContext = new LibraryDbContext(options);
            await _dbContext.Database.EnsureCreatedAsync();

            _genreRepository = new GenreRepository(_dbContext);
        }

        public async Task DisposeAsync()
        {
            await _pgContainer.DisposeAsync();
        }

        [Fact]
        public async Task AddAsync_ShouldAddGenre()
        {
            var genre = new Genre { Id = Guid.NewGuid(), Name = "Sci-fi" };

            await _genreRepository.AddAsync(genre);

            var result = await _dbContext.Genres.FindAsync(genre.Id);
            Assert.NotNull(result);
            Assert.Equal("Sci-fi", result!.Name);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllGenres()
        {
            var genre1 = new Genre { Id = Guid.NewGuid(), Name = "Fiction" };
            var genre2 = new Genre { Id = Guid.NewGuid(), Name = "Fantasy" };
            await _genreRepository.AddAsync(genre1);
            await _genreRepository.AddAsync(genre2);

            var result = await _genreRepository.GetAllAsync();

            Assert.Contains(result, g => g.Name == "Fiction");
            Assert.Contains(result, g => g.Name == "Fantasy");
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCorrectGenre()
        {
            var genre = new Genre { Id = Guid.NewGuid(), Name = "Horror" };
            await _genreRepository.AddAsync(genre);

            var result = await _genreRepository.GetByIdAsync(genre.Id);

            Assert.NotNull(result);
            Assert.Equal("Horror", result!.Name);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateGenre()
        {
            var genre = new Genre { Id = Guid.NewGuid(), Name = "Adventure" };
            await _genreRepository.AddAsync(genre);

            genre.Name = "Action Adventure";
            await _genreRepository.UpdateAsync(genre);

            var updated = await _genreRepository.GetByIdAsync(genre.Id);
            Assert.Equal("Action Adventure", updated!.Name);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteGenre()
        {
            var genre = new Genre { Id = Guid.NewGuid(), Name = "Drama" };
            await _genreRepository.AddAsync(genre);

            await _genreRepository.DeleteAsync(genre.Id);

            var deleted = await _genreRepository.GetByIdAsync(genre.Id);
            Assert.Null(deleted);
        }
    }
}
