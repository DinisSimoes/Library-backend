using Library.Application.Books.Commands;
using Library.Application.Exceptions;
using Library.Application.Interfaces;
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
    public class UpdateBookHandler : IRequestHandler<UpdateBookCommand, Unit>
    {
        private readonly IBookService _bookService;

        public UpdateBookHandler(IBookService bookService)
        {
            _bookService = bookService;
        }

        public async Task<Unit> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
        {
            var book = new Book
            {
                Id = request.Id,
                Title = request.Title,
                AuthorId = request.AuthorId,
                GenreId = request.GenreId
            };

            await _bookService.UpdateAsync(book);
            return Unit.Value;
        }
    }
}
