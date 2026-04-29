using LibraryManagement.Application.Common;
using LibraryManagement.Application.DTO;

namespace LibraryManagement.Application.Interfaces
{
    public interface IBookService
    {
        Task<ServiceResult<List<BookDto>>> GetAllBooksAsync();
        Task<ServiceResult<BookDto>> GetBookByIdAsync(int id);
        Task<ServiceResult<BookDto>> CreateBookAsync(CreateBookDto dto);
        Task<ServiceResult<BookDto>> UpdateBookAsync(int id, UpdateBookDto dto);
        Task<ServiceResult<bool>> DeleteBookAsync(int id);
        Task<ServiceResult<BorrowRecordDto>> BorrowBookAsync(int bookId, BorrowBookDto dto);
        Task<ServiceResult<BorrowRecordDto>> ReturnBookAsync(int bookId);
    }
}
