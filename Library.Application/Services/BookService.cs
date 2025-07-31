using Library.Application.Interfaces;
using Library.Domain.Entities;
using Library.Domain.Interfaces;

namespace Library.Application.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IValidationService _validationService;

        public BookService(IBookRepository bookRepository, IValidationService validationService)
        {
            _bookRepository = bookRepository;
            _validationService = validationService;
        }

        public async Task<IEnumerable<Book>> GetAllAsync()
        {
            return await _bookRepository.GetAllAsync();
        }

        public async Task<Book?> GetByIdAsync(Guid id)
        {
            return await _bookRepository.GetByIdAsync(id);
        }

        public async Task AddAsync(Book book)
        {
            await _validationService.ValidateAuthorExistAsync(book.AuthorId);
            await _validationService.ValidateGenreExistAsync(book.GenreId);

            await _bookRepository.AddAsync(book);
        }

        public async Task UpdateAsync(Book book)
        {
            await _bookRepository.UpdateAsync(book);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _bookRepository.DeleteAsync(id);
        }
    }
}
