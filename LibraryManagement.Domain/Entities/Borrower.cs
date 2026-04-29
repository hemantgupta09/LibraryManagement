using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryManagement.Domain.Entities
{
    public class Borrower
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public List<BorrowRecord> BorrowRecords { get; set; } = new List<BorrowRecord>();
    }
}
