namespace Library.Domain.Entities
{
    public class Book
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = default!;
        public Guid GenreId { get; set; }
        public Genre Genre { get; set; } = default!;
        public Guid AuthorId { get; set; }
        public Author Author { get; set; } = default!;
    }
}
