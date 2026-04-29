using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Interfaces;
using LibraryManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Infrastructure.Repositories
{
    public class BorrowerRepository : Repository<Borrower>, IBorrowerRepository
    {
        public BorrowerRepository(LibraryDbContext context) : base(context) { }

        public async Task<bool> EmailAlreadyExistsAsync(string email, int? excludeBorrowerId = null)
        {
            var query = _context.Borrowers.AsQueryable();
            if (excludeBorrowerId.HasValue)
                query = query.Where(b => b.Email == email && b.Id != excludeBorrowerId.Value);
            else
                query = query.Where(b => b.Email == email);

            return await query.AnyAsync();
        }
        public async Task<List<BorrowRecord>> GetBorrowHistoryAsync(int borrowerId)
        {
            var query = _context.BorrowRecords.AsNoTracking().Where(r => r.BorrowerId == borrowerId).OrderByDescending(r => r.BorrowedAt).AsQueryable();

            query = query.Include(r => r.Book);
            query = query.Include(r => r.Borrower);

            return await query.ToListAsync();
        }
    }
}
