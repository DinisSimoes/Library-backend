using Library.Application.Authors.Commands;
using Library.Application.Interfaces;
using Library.Domain.Entities;
using Library.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Authors.Handlers
{
    public class CreateAuthorHandler : IRequestHandler<CreateAuthorCommand, Guid>
    {
        private readonly IAuthorService _authorService;

        public CreateAuthorHandler(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        public async Task<Guid> Handle(CreateAuthorCommand request, CancellationToken cancellationToken)
        {
            var author = new Author
            {
                Id = Guid.NewGuid(),
                Name = request.Name
            };

            await _authorService.AddAsync(author);
            return author.Id;
        }
    }
}
