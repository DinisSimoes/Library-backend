using Library.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Authors.Queries
{
    public record GetAuthorByIdQuery(Guid Id) : IRequest<AuthorDto?>;
}
