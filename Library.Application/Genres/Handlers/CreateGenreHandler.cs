using Library.Application.Genres.Commands;
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
    public class CreateGenreHandler : IRequestHandler<CreateGenreCommand, Guid>
    {
        private readonly IGenreService _genreService;

        public CreateGenreHandler(IGenreService genreService)
        {
            _genreService = genreService;
        }

        public async Task<Guid> Handle(CreateGenreCommand request, CancellationToken cancellationToken)
        {
            var genre = new Genre
            {
                Id = Guid.NewGuid(),
                Name = request.Name
            };

            await _genreService.AddAsync(genre);
            return genre.Id;
        }
    }
}
