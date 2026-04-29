namespace LibraryManagement.Application.DTO
{
    public class BookDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public int PublicationYear { get; set; }
        public string? Genre { get; set; }
        public bool IsAvailable { get; set; }
        public int AuthorId { get; set; }
        public string AuthorName { get; set; } = string.Empty;
    }
}
