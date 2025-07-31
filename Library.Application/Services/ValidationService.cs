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
    public class ValidationService : IValidationService
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IGenreRepository _genreRepository;

        public ValidationService(IAuthorRepository authorRepository, IGenreRepository genreRepository)
        {
            _authorRepository = authorRepository;
            _genreRepository = genreRepository;
        }

        public async Task ValidateAuthorExistAsync(Guid authorId)
        {
            var author = await _authorRepository.GetByIdAsync(authorId);
            if (author == null)
                throw new NotFoundException(nameof(Author), authorId);
        }

        public async Task ValidateGenreExistAsync(Guid genreId)
        {
            var genre = await _genreRepository.GetByIdAsync(genreId);
            if (genre == null)
                throw new NotFoundException(nameof(Genre), genreId);
        }
    }
}
