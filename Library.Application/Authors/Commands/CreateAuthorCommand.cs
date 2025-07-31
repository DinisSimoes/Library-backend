using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Authors.Commands
{
    public record CreateAuthorCommand(string Name) : IRequest<Guid>;
}
