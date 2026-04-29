using FluentAssertions;
using LibraryManagement.Application.DTO;
using LibraryManagement.Application.Services;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Infrastructure.Persistence;
using LibraryManagement.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LibraryManagement.Tests
{
    public class AddBookTests
    {
        private readonly LibraryDbContext _dbContext;
        private readonly BookService _bookService;

        public AddBookTests()
        {
            var options = new DbContextOptionsBuilder<LibraryDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            _dbContext = new LibraryDbContext(options);

            var bookRepository = new BookRepository(_dbContext);
            var authorRepository = new AuthorRepository(_dbContext);
            var borrowerRepository = new BorrowerRepository(_dbContext);

            _bookService = new BookService(bookRepository, authorRepository, borrowerRepository);
        }

        [Fact]
        public async Task CreateBook_IncreasesBookCountByOne()
        {
            var author = new Author { FirstName = "Frank", LastName = "Herbert" };

            await _dbContext.Authors.AddAsync(author);
            await _dbContext.SaveChangesAsync();

            var countBefore = _dbContext.Books.Count();

            var createBookDto = new CreateBookDto
            {
                Title = "Dune",
                ISBN = "978-0-34-196600-3",
                PublicationYear = 1965,
                Genre = "Science Fiction",
                AuthorId = author.Id
            };

            var result = await _bookService.CreateBookAsync(createBookDto);

            result.IsSuccess.Should().BeTrue();
            _dbContext.Books.Count().Should().Be(countBefore + 1);
        }

        [Fact]
        public async Task CreateBook_WithNonExistentAuthor_Fails()
        {
            var dto = new CreateBookDto
            {
                Title = "Ghost Book",
                ISBN = "000-0-00-000000-0",
                PublicationYear = 2020,
                AuthorId = 9999
            };

            var result = await _bookService.CreateBookAsync(dto);

            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Contain("Author");
        }

        [Fact]
        public async Task CreateBook_WithDuplicateISBN_Fails()
        {
            var author = new Author { FirstName = "George", LastName = "Orwell" };

            await _dbContext.Authors.AddAsync(author);
            await _dbContext.SaveChangesAsync();

            var firstBook = new CreateBookDto
            {
                Title = "1984",
                ISBN = "978-0-45-228285-3",
                PublicationYear = 1949,
                AuthorId = author.Id
            };
            await _bookService.CreateBookAsync(firstBook);

            var duplicateISBN = new CreateBookDto
            {
                Title = "Animal Farm",
                ISBN = "978-0-45-228285-3",
                PublicationYear = 1945,
                AuthorId = author.Id
            };

            var result = await _bookService.CreateBookAsync(duplicateISBN);

            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Contain("ISBN");
        }
        public void Dispose() => _dbContext.Dispose();
    }
}
