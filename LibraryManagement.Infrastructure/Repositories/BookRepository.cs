using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Interfaces;
using LibraryManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Infrastructure.Repositories
{
    public class BookRepository : Repository<Book>, IBookRepository
    {
        public BookRepository(LibraryDbContext context) : base(context) { }

        public async Task<Book?> GetBookWithDetailsAsync(int bookId)
        {
            var book = await _context.Books.Include(b => b.Author).Include(b => b.BorrowRecords).ThenInclude(r => r.Borrower).FirstOrDefaultAsync(b => b.Id == bookId);

            return book;
        }

        public async Task<bool> IsbnAlreadyExistsAsync(string isbn, int? excludeBookId = null)
            => await _context.Books.AnyAsync(b =>
                b.ISBN == isbn &&
                (excludeBookId == null || b.Id != excludeBookId));
    }

}
