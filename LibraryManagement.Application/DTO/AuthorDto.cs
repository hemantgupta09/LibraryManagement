namespace LibraryManagement.Application.DTO
{
    public class AuthorDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string? Biography { get; set; }
        public DateOnly? DateOfBirth { get; set; }
    }
}
