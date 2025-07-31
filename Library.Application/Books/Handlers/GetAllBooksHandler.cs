using Library.Application.Books.Queries;
using Library.Application.Interfaces;
using Library.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Books.Handlers
{
    public class GetAllBooksHandler : IRequestHandler<GetAllBooksQuery, IEnumerable<BookDto>>
    {
        private readonly IBookService _bookService;

        public GetAllBooksHandler(IBookService bookService)
        {
            _bookService = bookService;
        }

        public async Task<IEnumerable<BookDto>> Handle(GetAllBooksQuery request, CancellationToken cancellationToken)
        {
            var books = await _bookService.GetAllAsync();

            return books.Select(b => new BookDto
            {
                Id = b.Id,
                Title = b.Title,
                AuthorId = b.AuthorId,
                AuthorName = b.Author.Name,
                GenreId = b.GenreId,
                GenreName = b.Genre.Name
            });
        }
    }
}
