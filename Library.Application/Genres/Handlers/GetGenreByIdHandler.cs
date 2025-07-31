using Library.Application.Genres.Queries;
using Library.Application.Interfaces;
using Library.Domain.Entities;
using Library.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Genres.Handlers
{
    public class GetGenreByIdHandler : IRequestHandler<GetGenreByIdQuery, GenreDto?>
    {
        private readonly IGenreService _genreService;

        public GetGenreByIdHandler(IGenreService genreService)
        {
            _genreService = genreService;
        }

        public async Task<GenreDto?> Handle(GetGenreByIdQuery request, CancellationToken cancellationToken)
        {
            var genre = await _genreService.GetByIdAsync(request.Id);
            if (genre == null) { return null; }

            return new GenreDto
            {
                Id = genre.Id,
                Name = genre.Name
            };
        }
    }
}
