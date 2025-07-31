using Library.Domain.Entities;
using MediatR;

namespace Library.Application.Books.Queries
{
    public record GetAllBooksQuery() : IRequest<IEnumerable<BookDto>>;
}
