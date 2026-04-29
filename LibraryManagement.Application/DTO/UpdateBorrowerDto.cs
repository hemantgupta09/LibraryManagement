using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Application.DTO
{
    public class UpdateBorrowerDto
    {
        [Required(ErrorMessage = "Borrower Name is required.")]
        [MaxLength(100, ErrorMessage = "Borrower Name cannot exceed 100 characters.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Borrower Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } = string.Empty;

        [StringLength(10, MinimumLength = 10, ErrorMessage = "Phone number must be exactly 10 digits.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must contain only digits.")]
        public string? PhoneNumber { get; set; }
    }
}
