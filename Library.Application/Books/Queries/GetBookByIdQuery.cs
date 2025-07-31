using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Books.Queries
{
    public record GetBookByIdQuery(Guid Id) : IRequest<BookDto?>;
}
