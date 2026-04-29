namespace LibraryManagement.Domain.Entities
{
    public class BorrowRecord
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; } = null!;
        public int BorrowerId { get; set; }
        public Borrower Borrower { get; set; } = null!;
        public DateTime BorrowedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ReturnedAt { get; set; }
        public bool IsReturned
        {
            get
            {
                return ReturnedAt.HasValue;
            }
        }
    }
}
