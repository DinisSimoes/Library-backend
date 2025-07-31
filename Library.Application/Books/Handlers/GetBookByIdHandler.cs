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
    public class GetBookByIdHandler : IRequestHandler<GetBookByIdQuery, BookDto?>
    {
        private readonly IBookService _bookService;

        public GetBookByIdHandler(IBookService bookService)
        {
            _bookService = bookService;
        }

        public async Task<BookDto?> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
        {
            var book = await _bookService.GetByIdAsync(request.Id);
            if (book == null) return null;

            return new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                AuthorName = book.Author.Name,
                GenreName = book.Genre.Name
            };
        }
    }
}
