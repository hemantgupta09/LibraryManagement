using LibraryManagement.Application.Common;
using LibraryManagement.Application.DTO;
using LibraryManagement.Application.Interfaces.Services;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Interfaces;

namespace LibraryManagement.Application.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly IBorrowerRepository _borrowerRepository;
        public BookService(IBookRepository bookRepository, IAuthorRepository authorRepository, IBorrowerRepository borrowerRepository)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _borrowerRepository = borrowerRepository;
        }
        public async Task<ServiceResult<List<BookDto>>> GetAllBooksAsync()
        {
            var books = await _bookRepository.GetAllAsync();
            return ServiceResult<List<BookDto>>.Success(books.Select(MapToBookDto).ToList());
        }
        public async Task<ServiceResult<BookDto>> GetBookByIdAsync(int id)
        {
            var book = await _bookRepository.GetBookWithDetailsAsync(id);

            if (book == null)
                return ServiceResult<BookDto>.Failure($"Book with ID {id} was not found.");

            return ServiceResult<BookDto>.Success(MapToBookDto(book));
        }
        public async Task<ServiceResult<BookDto>> CreateBookAsync(CreateBookDto dto)
        {
            var author = await _authorRepository.GetByIdAsync(dto.AuthorId);
            if (author == null)
                return ServiceResult<BookDto>.Failure($"Author with ID {dto.AuthorId} was not found.");

            bool isbnTaken = await _bookRepository.IsbnAlreadyExistsAsync(dto.ISBN);
            if (isbnTaken)
                return ServiceResult<BookDto>.Failure($"A book with ISBN '{dto.ISBN}' already exists.");

            var book = new Book
            {
                Title = dto.Title.Trim(),
                ISBN = dto.ISBN.Trim(),
                PublicationYear = dto.PublicationYear,
                Genre = dto.Genre?.Trim(),
                AuthorId = dto.AuthorId,
                Author = author,
                IsAvailable = true
            };

            await _bookRepository.AddAsync(book);
            await _bookRepository.SaveChangesAsync();
            return ServiceResult<BookDto>.Success(MapToBookDto(book));
        }
        public async Task<ServiceResult<BookDto>> UpdateBookAsync(int id, UpdateBookDto dto)
        {
            var book = await _bookRepository.GetBookWithDetailsAsync(id);
            if (book == null)
                return ServiceResult<BookDto>.Failure($"Book with ID {id} was not found.");

            var author = await _authorRepository.GetByIdAsync(dto.AuthorId);
            if (author == null)
                return ServiceResult<BookDto>.Failure($"Author with ID {dto.AuthorId} was not found.");

            bool isbnTaken = await _bookRepository.IsbnAlreadyExistsAsync(dto.ISBN, excludeBookId: id);
            if (isbnTaken)
                return ServiceResult<BookDto>.Failure($"Another book already uses the ISBN '{dto.ISBN}'.");

            book.Title = dto.Title.Trim();
            book.ISBN = dto.ISBN.Trim();
            book.PublicationYear = dto.PublicationYear;
            book.Genre = dto.Genre?.Trim();
            book.AuthorId = dto.AuthorId;
            book.Author = author;

            _bookRepository.Update(book);
            await _bookRepository.SaveChangesAsync();

            return ServiceResult<BookDto>.Success(MapToBookDto(book));
        }
        public async Task<ServiceResult<bool>> DeleteBookAsync(int id)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            if (book == null)
                return ServiceResult<bool>.Failure($"Book with ID {id} was not found.");

            _bookRepository.Delete(book);
            await _bookRepository.SaveChangesAsync();

            return ServiceResult<bool>.Success(true);
        }
        public async Task<ServiceResult<BorrowRecordDto>> BorrowBookAsync(int bookId, BorrowBookDto dto)
        {
            var book = await _bookRepository.GetBookWithDetailsAsync(bookId);
            if (book == null)
                return ServiceResult<BorrowRecordDto>.Failure($"Book with ID {bookId} was not found.");

            if (!book.IsAvailable)
                return ServiceResult<BorrowRecordDto>.Failure($"'{book.Title}' is already borrowed and not available.");

            var borrower = await _borrowerRepository.GetByIdAsync(dto.BorrowerId);
            if (borrower == null)
                return ServiceResult<BorrowRecordDto>.Failure($"Borrower with ID {dto.BorrowerId} was not found.");

            book.IsAvailable = false;

            var record = new BorrowRecord
            {
                BookId = bookId,
                BorrowerId = dto.BorrowerId,
                BorrowedAt = DateTime.UtcNow,
                Book = book,
                Borrower = borrower
            };

            book.BorrowRecords.Add(record);
            _bookRepository.Update(book);
            await _bookRepository.SaveChangesAsync();

            return ServiceResult<BorrowRecordDto>.Success(MapToRecordDto(record));
        }
        public async Task<ServiceResult<BorrowRecordDto>> ReturnBookAsync(int bookId)
        {
            var book = await _bookRepository.GetBookWithDetailsAsync(bookId);
            if (book == null)
                return ServiceResult<BorrowRecordDto>.Failure($"Book with ID {bookId} was not found.");

            if (book.IsAvailable)
                return ServiceResult<BorrowRecordDto>.Failure($"'{book.Title}' is not currently borrowed.");
            var activeRecord = book.BorrowRecords.FirstOrDefault(r => r.ReturnedAt == null);
            if (activeRecord == null)
                return ServiceResult<BorrowRecordDto>.Failure("No active borrow record found for this book.");

            activeRecord.ReturnedAt = DateTime.UtcNow;
            book.IsAvailable = true;

            _bookRepository.Update(book);
            await _bookRepository.SaveChangesAsync();

            return ServiceResult<BorrowRecordDto>.Success(MapToRecordDto(activeRecord));
        }
        private static BookDto MapToBookDto(Book book) => new BookDto
        {
            Id = book.Id,
            Title = book.Title,
            ISBN = book.ISBN,
            PublicationYear = book.PublicationYear,
            Genre = book.Genre,
            IsAvailable = book.IsAvailable,
            AuthorId = book.AuthorId,
            AuthorName = book.Author?.FullName ?? string.Empty
        };

        private static BorrowRecordDto MapToRecordDto(BorrowRecord record) => new BorrowRecordDto
        {
            Id = record.Id,
            BookId = record.BookId,
            BookTitle = record.Book?.Title ?? string.Empty,
            BorrowerId = record.BorrowerId,
            BorrowerName = record.Borrower?.Name ?? string.Empty,
            BorrowedAt = record.BorrowedAt,
            ReturnedAt = record.ReturnedAt,
            IsReturned = record.IsReturned
        };
    }
}
