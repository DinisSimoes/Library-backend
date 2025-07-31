using Library.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Interfaces
{
    public interface IAuthorService
    {
        Task<IEnumerable<Author>> GetAllAsync();
        Task<Author?> GetByIdAsync(Guid id);
        Task AddAsync(Author author);
        Task UpdateAsync(Author author);
        Task DeleteAsync(Guid id);
    }
}
