using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Domain.Interfaces
{
    public interface IBookRepository : IRepository<Book>
    {
        Task<Book?> GetBookWithDetailsAsync(int bookId);
        Task<bool> IsbnAlreadyExistsAsync(string isbn, int? excludeBookId = null);
    }
}
