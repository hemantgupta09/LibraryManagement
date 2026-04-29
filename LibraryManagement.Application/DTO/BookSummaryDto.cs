namespace LibraryManagement.Application.DTO
{
    public class BookSummaryDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public bool IsAvailable { get; set; }
    }
}
