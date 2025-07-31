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
    public class GetAllAuthorsHandler : IRequestHandler<GetAllAuthorsQuery, IEnumerable<AuthorDto>>
    {
        private readonly IAuthorService _authorService;

        public GetAllAuthorsHandler(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        public async Task<IEnumerable<AuthorDto>> Handle(GetAllAuthorsQuery request, CancellationToken cancellationToken)
        {
            var authors = await _authorService.GetAllAsync();
            return authors.Select(author => new AuthorDto
            {
                Id = author.Id,
                Name = author.Name,
            });
        }
    }
}
