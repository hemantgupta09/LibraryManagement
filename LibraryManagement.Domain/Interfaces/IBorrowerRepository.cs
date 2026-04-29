using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Domain.Interfaces
{
    public interface IBorrowerRepository : IRepository<Borrower>
    {
        Task<bool> EmailAlreadyExistsAsync(string email, int? excludeBorrowerId = null);
        Task<List<BorrowRecord>> GetBorrowHistoryAsync(int borrowerId);
    }
}
