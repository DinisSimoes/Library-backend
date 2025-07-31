using Library.Application.Books.Commands;
using Library.Application.Interfaces;
using Library.Application.Services;
using Library.Domain.Entities;
using Library.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Books.Handlers
{
    public class CreateBookHandler : IRequestHandler<CreateBookCommand, Guid>
    {
        private readonly IBookService _bookService;

        public CreateBookHandler(IBookService service)
        {
            _bookService = service;
        }

        public async Task<Guid> Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {
            var book = new Book
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                AuthorId = request.AuthorId,
                GenreId = request.GenreId
            };

            await _bookService.AddAsync(book);
            return book.Id;
        }
    }
}
