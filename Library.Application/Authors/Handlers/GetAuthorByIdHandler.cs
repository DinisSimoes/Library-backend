using Library.Application.Authors.Queries;
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
    public class GetAuthorByIdHandler : IRequestHandler<GetAuthorByIdQuery, AuthorDto?>
    {
        private readonly IAuthorService _authorService;

        public GetAuthorByIdHandler(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        public async Task<AuthorDto?> Handle(GetAuthorByIdQuery request, CancellationToken cancellationToken)
        {
            var author = await _authorService.GetByIdAsync(request.Id);
            if (author == null) { return null; }

            return new AuthorDto
            {
                Id = author.Id,
                Name = author.Name,
            };
        }
    }
}
