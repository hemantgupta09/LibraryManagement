using LibraryManagement.Application.Common;
using LibraryManagement.Application.DTO;
using LibraryManagement.Application.Interfaces.Services;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Interfaces;

namespace LibraryManagement.Application.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _authorRepository;
        public AuthorService(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }
        public async Task<ServiceResult<List<AuthorDto>>> GetAllAuthorsAsync()
        {
            var authors = await _authorRepository.GetAllAsync();

            var dtos = authors.Select(MapToAuthorDto).ToList();

            return ServiceResult<List<AuthorDto>>.Success(dtos);
        }
        public async Task<ServiceResult<AuthorWithBooksDto>> GetAuthorByIdAsync(int id)
        {
            var author = await _authorRepository.GetAuthorWithBooksAsync(id);

            if (author == null)
                return ServiceResult<AuthorWithBooksDto>.Failure($"Author with ID {id} was not found.");

            var dto = new AuthorWithBooksDto
            {
                Id = author.Id,
                FullName = author.FullName,
                Biography = author.Biography,
                DateOfBirth = author.DateOfBirth,
                Books = author.Books.Select(b => new BookSummaryDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    ISBN = b.ISBN,
                    IsAvailable = b.IsAvailable
                }).ToList()
            };

            return ServiceResult<AuthorWithBooksDto>.Success(dto);
        }
        public async Task<ServiceResult<AuthorDto>> CreateAuthorAsync(CreateAuthorDto dto)
        {
            var author = new Author
            {
                FirstName = dto.FirstName.Trim(),
                LastName = dto.LastName.Trim(),
                Biography = dto.Biography?.Trim(),
                DateOfBirth = dto.DateOfBirth
            };

            await _authorRepository.AddAsync(author);
            await _authorRepository.SaveChangesAsync();

            return ServiceResult<AuthorDto>.Success(MapToAuthorDto(author));
        }
        public async Task<ServiceResult<AuthorDto>> UpdateAuthorAsync(int id, UpdateAuthorDto dto)
        {
            var author = await _authorRepository.GetByIdAsync(id);

            author.FirstName = dto.FirstName.Trim();
            author.LastName = dto.LastName.Trim();
            author.Biography = dto.Biography?.Trim();
            author.DateOfBirth = dto.DateOfBirth;

            _authorRepository.Update(author);
            await _authorRepository.SaveChangesAsync();

            return ServiceResult<AuthorDto>.Success(MapToAuthorDto(author));
        }
        public async Task<ServiceResult<bool>> DeleteAuthorAsync(int id)
        {
            var author = await _authorRepository.GetByIdAsync(id);

            if (author == null)
                return ServiceResult<bool>.Failure($"Author with ID {id} was not found.");

            _authorRepository.Delete(author);
            await _authorRepository.SaveChangesAsync();

            return ServiceResult<bool>.Success(true);
        }

        private static AuthorDto MapToAuthorDto(Author author) => new AuthorDto
        {
            Id = author.Id,
            FirstName = author.FirstName,
            LastName = author.LastName,
            FullName = author.FullName,
            Biography = author.Biography,
            DateOfBirth = author.DateOfBirth
        };
    }
}
