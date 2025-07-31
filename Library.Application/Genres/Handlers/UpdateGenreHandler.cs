using Library.Application.Genres.Commands;
using Library.Application.Interfaces;
using Library.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Genres.Handlers
{
    public class UpdateGenreHandler : IRequestHandler<UpdateGenreCommand, Unit>
    {
        private readonly IGenreService _genreService;

        public UpdateGenreHandler(IGenreService genreService)
        {
            _genreService = genreService;
        }

        public async Task<Unit> Handle(UpdateGenreCommand request, CancellationToken cancellationToken)
        {
            var genre = await _genreService.GetByIdAsync(request.Id)
                         ?? throw new KeyNotFoundException("Genre not found.");

            genre.Name = request.Name;
            await _genreService.UpdateAsync(genre);

            return Unit.Value;
        }
    }
}
