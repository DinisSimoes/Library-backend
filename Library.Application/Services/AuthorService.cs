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
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _authorRepository;

        public AuthorService(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public async Task<IEnumerable<Author>> GetAllAsync()
        {
            return await _authorRepository.GetAllAsync();
        }

        public async Task<Author?> GetByIdAsync(Guid id)
        {
            var author = await _authorRepository.GetByIdAsync(id);
            if (author is null)
                throw new NotFoundException("Author", id);

            return author;
        }

        public async Task AddAsync(Author author)
        {
            await _authorRepository.AddAsync(author);
        }

        public async Task UpdateAsync(Author author)
        {
            var existingAuthor = await _authorRepository.GetByIdAsync(author.Id);
            if (existingAuthor is null)
                throw new NotFoundException("Author", author.Id);

            await _authorRepository.UpdateAsync(author);
        }


        public async Task DeleteAsync(Guid id)
        {
            var author = await _authorRepository.GetByIdAsync(id);
            if (author == null)
                throw new NotFoundException(nameof(Author));

            if (author.Books.Any())
                throw new ArgumentException("Não é possível deletar o autor pois há livros associados.");

            await _authorRepository.DeleteAsync(id);
        }
    }
}
