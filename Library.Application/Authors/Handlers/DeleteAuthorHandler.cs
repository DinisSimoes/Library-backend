using Library.Application.Authors.Commands;
using Library.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Authors.Handlers
{
    public class DeleteAuthorHandler : IRequestHandler<DeleteAuthorCommand, Unit>
    {
        private readonly IAuthorService _authorService;

        public DeleteAuthorHandler(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        public async Task<Unit> Handle(DeleteAuthorCommand request, CancellationToken cancellationToken)
        {
            await _authorService.DeleteAsync(request.Id);
            return Unit.Value;
        }
    }
}
