using Library.Application.Exceptions;
using Library.Application.Interfaces;
using Library.Domain.Entities;
using Library.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Services
{
    public class GenreService : IGenreService
    {
        private readonly IGenreRepository _genreRepository;

        public GenreService(IGenreRepository genreRepository)
        {
            _genreRepository = genreRepository;
        }

        public async Task<IEnumerable<Genre>> GetAllAsync()
        {
            return await _genreRepository.GetAllAsync();
        }

        public async Task<Genre?> GetByIdAsync(Guid id)
        {
            return await _genreRepository.GetByIdAsync(id);
        }

        public async Task AddAsync(Genre genre)
        {
            await _genreRepository.AddAsync(genre);
        }

        public async Task UpdateAsync(Genre genre)
        {
            await _genreRepository.UpdateAsync(genre);
        }

        public async Task DeleteAsync(Guid id)
        {
            var genre = await _genreRepository.GetByIdAsync(id);
            if (genre == null)
                throw new NotFoundException(nameof(Genre));

            if (genre.Books.Any())
                throw new ArgumentException("Não é possível deletar o gênero pois há livros associados.");

            await _genreRepository.DeleteAsync(id);
        }
    }
}
