using LibraryManagement.Application.Common;
using LibraryManagement.Application.DTO;

namespace LibraryManagement.Application.Interfaces
{
    public interface IAuthorService
    {
        Task<ServiceResult<List<AuthorDto>>> GetAllAuthorsAsync();
        Task<ServiceResult<AuthorWithBooksDto>> GetAuthorByIdAsync(int id);
        Task<ServiceResult<AuthorDto>> CreateAuthorAsync(CreateAuthorDto request);
        Task<ServiceResult<AuthorDto>> UpdateAuthorAsync(int id, UpdateAuthorDto request);
        Task<ServiceResult<bool>> DeleteAuthorAsync(int id);
    }
}
