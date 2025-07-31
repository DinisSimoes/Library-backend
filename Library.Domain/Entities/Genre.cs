namespace Library.Domain.Entities
{
    public class Genre
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
