using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Interfaces
{
    public interface IValidationService
    {
        Task ValidateAuthorExistAsync(Guid authorId);
        Task ValidateGenreExistAsync(Guid genreId);

    }
}
