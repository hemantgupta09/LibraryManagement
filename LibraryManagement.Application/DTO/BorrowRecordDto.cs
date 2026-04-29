namespace LibraryManagement.Application.DTO
{
    public class BorrowRecordDto
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public string BookTitle { get; set; } = string.Empty;
        public int BorrowerId { get; set; }
        public string BorrowerName { get; set; } = string.Empty;
        public DateTime BorrowedAt { get; set; }
        public DateTime? ReturnedAt { get; set; }
        public bool IsReturned { get; set; }
    }
}
