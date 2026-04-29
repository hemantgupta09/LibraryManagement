using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Application.DTO
{
    public class UpdateBookDto
    {
        [Required(ErrorMessage = "Book Title is required.")]
        [MaxLength(100, ErrorMessage = "Book Title cannot exceed 100 characters.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "International Standard Book Number (ISBN) is required.")]
        [MaxLength(100, ErrorMessage = "International Standard Book Number (ISBN) cannot exceed 100 characters.")]
        public string ISBN { get; set; } = string.Empty;

        [Range(1000, 9999, ErrorMessage = "Publication year must be between 1000 and 9999.")]
        public int PublicationYear { get; set; }


        [MaxLength(100, ErrorMessage = "Genre cannot exceed 100 characters.")]
        public string? Genre { get; set; }

        [Required(ErrorMessage = "Author ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Author ID must be greater than zero.")]
        public int AuthorId { get; set; }
    }
}
