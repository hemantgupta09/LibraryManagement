using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Application.DTO
{
    public class BorrowBookDto
    {
        [Required(ErrorMessage = "Book Id is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Author id must be greater than zero")]
        public int BorrowerId { get; set; }
    }
}
