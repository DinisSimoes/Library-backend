using Library.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Genres.Queries
{
    public record GetGenreByIdQuery(Guid Id) : IRequest<GenreDto?>;
}
