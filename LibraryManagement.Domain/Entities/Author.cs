namespace LibraryManagement.Domain.Entities
{
    public class Author
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Biography { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public List<Book> Books { get; set; } = new List<Book>();
        public string FullName { get { return $"{FirstName} {LastName}"; } }
    };
}
