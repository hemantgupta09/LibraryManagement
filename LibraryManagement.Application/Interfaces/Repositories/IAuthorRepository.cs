using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Domain.Interfaces
{
    public interface IAuthorRepository : IRepository<Author>
    {
        Task<Author?> GetAuthorWithBooksAsync(int authorId);
    }
}
