using Library.Application.Authors;
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
    public class GetAllGenresHandler : IRequestHandler<GetAllGenresQuery, IEnumerable<GenreDto>>
    {
        private readonly IGenreService _genreService;

        public GetAllGenresHandler(IGenreService genreService)
        {
            _genreService = genreService;
        }

        public async Task<IEnumerable<GenreDto>> Handle(GetAllGenresQuery request, CancellationToken cancellationToken)
        {
            var genres = await _genreService.GetAllAsync();
            return genres.Select(genre => new GenreDto
            {
                Id = genre.Id,
                Name = genre.Name,
            });
        }
    }
}
