namespace LibraryManagement.Application.DTO
{
    public class AuthorWithBooksDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string? Biography { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public List<BookSummaryDto> Books { get; set; } = new();
    }
}
