using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Books
{
    public class BookDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = default!;
        public Guid AuthorId { get; set; }
        public string AuthorName { get; set; } = default!;
        public Guid GenreId { get; set; }
        public string GenreName { get; set; } = default!;
    }
}
