using LibraryManagement.Application.Common;
using LibraryManagement.Application.DTO;

namespace LibraryManagement.Application.Interfaces.Services
{
    public interface IBorrowerService
    {
        Task<ServiceResult<List<BorrowerDto>>> GetAllBorrowersAsync();
        Task<ServiceResult<BorrowerDto>> GetBorrowerByIdAsync(int id);
        Task<ServiceResult<BorrowerDto>> CreateBorrowerAsync(CreateBorrowerDto dto);
        Task<ServiceResult<BorrowerDto>> UpdateBorrowerAsync(int id, UpdateBorrowerDto dto);
        Task<ServiceResult<bool>> DeleteBorrowerAsync(int id);
        Task<ServiceResult<List<BorrowRecordDto>>> GetBorrowerHistoryAsync(int borrowerId);
    }
}
