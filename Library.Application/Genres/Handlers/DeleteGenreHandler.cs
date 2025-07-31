using Library.Application.Genres.Commands;
using Library.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Genres.Handlers
{
    public class DeleteGenreHandler : IRequestHandler<DeleteGenreCommand, Unit>
    {
        private readonly IGenreService _genreService;

        public DeleteGenreHandler(IGenreService genreService)
        {
            _genreService = genreService;
        }

        public async Task<Unit> Handle(DeleteGenreCommand request, CancellationToken cancellationToken)
        {
            await _genreService.DeleteAsync(request.Id);
            return Unit.Value;
        }
    }
}
