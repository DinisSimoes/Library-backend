using Library.Application.Authors.Commands;
using Library.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Authors.Handlers
{
    public class UpdateAuthorHandler : IRequestHandler<UpdateAuthorCommand, Unit>
    {
        private readonly IAuthorRepository _repository;

        public UpdateAuthorHandler(IAuthorRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(UpdateAuthorCommand request, CancellationToken cancellationToken)
        {
            var author = await _repository.GetByIdAsync(request.Id)
                         ?? throw new KeyNotFoundException("Autor não encontrado.");

            author.Name = request.Name;
            await _repository.UpdateAsync(author);

            return Unit.Value;
        }
    }
}
