using Library.Application.Books.Commands;
using Library.Application.Exceptions;
using Library.Application.Interfaces;
using Library.Domain.Entities;
using Library.Domain.Interfaces;
using MediatR;

namespace Library.Application.Books.Handlers
{
    public class DeleteBookHandler : IRequestHandler<DeleteBookCommand, Unit>
    {
        private readonly IBookService _bookService;

        public DeleteBookHandler(IBookService bookService)
        {
            _bookService = bookService;
        }

        public async Task<Unit> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
            await _bookService.DeleteAsync(request.Id);
            return Unit.Value;
        }
    }
}
