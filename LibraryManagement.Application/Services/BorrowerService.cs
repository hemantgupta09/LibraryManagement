using LibraryManagement.Application.Common;
using LibraryManagement.Application.DTO;
using LibraryManagement.Application.Interfaces.Services;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Interfaces;

namespace LibraryManagement.Application.Services
{
    public class BorrowerService : IBorrowerService
    {
        private readonly IBorrowerRepository _borrowerRepository;
        public BorrowerService(IBorrowerRepository borrowerRepository)
        {
            _borrowerRepository = borrowerRepository;
        }
        public async Task<ServiceResult<List<BorrowerDto>>> GetAllBorrowersAsync()
        {
            var borrowers = await _borrowerRepository.GetAllAsync();
            return ServiceResult<List<BorrowerDto>>.Success(borrowers.Select(MapToBorrowerDto).ToList());
        }
        public async Task<ServiceResult<BorrowerDto>> GetBorrowerByIdAsync(int id)
        {
            var borrower = await _borrowerRepository.GetByIdAsync(id);

            if (borrower == null)
                return ServiceResult<BorrowerDto>.Failure($"Borrower with ID {id} was not found.");

            return ServiceResult<BorrowerDto>.Success(MapToBorrowerDto(borrower));
        }
        public async Task<ServiceResult<BorrowerDto>> CreateBorrowerAsync(CreateBorrowerDto dto)
        {
            bool emailTaken = await _borrowerRepository.EmailAlreadyExistsAsync(dto.Email);
            if (emailTaken)
                return ServiceResult<BorrowerDto>.Failure($"A borrower with email '{dto.Email}' already exists.");

            var borrower = new Borrower
            {
                Name = dto.Name.Trim(),
                Email = dto.Email.Trim().ToLowerInvariant(),
                PhoneNumber = dto.PhoneNumber?.Trim()
            };

            await _borrowerRepository.AddAsync(borrower);
            await _borrowerRepository.SaveChangesAsync();

            return ServiceResult<BorrowerDto>.Success(MapToBorrowerDto(borrower));
        }
        public async Task<ServiceResult<BorrowerDto>> UpdateBorrowerAsync(int id, UpdateBorrowerDto dto)
        {
            var borrower = await _borrowerRepository.GetByIdAsync(id);
            if (borrower == null)
                return ServiceResult<BorrowerDto>.Failure($"Borrower with ID {id} was not found.");

            bool emailTaken = await _borrowerRepository.EmailAlreadyExistsAsync(dto.Email, excludeBorrowerId: id);
            if (emailTaken)
                return ServiceResult<BorrowerDto>.Failure($"Another borrower already uses the email '{dto.Email}'.");

            borrower.Name = dto.Name.Trim();
            borrower.Email = dto.Email.Trim().ToLowerInvariant();
            borrower.PhoneNumber = dto.PhoneNumber?.Trim();

            _borrowerRepository.Update(borrower);
            await _borrowerRepository.SaveChangesAsync();

            return ServiceResult<BorrowerDto>.Success(MapToBorrowerDto(borrower));
        }
        public async Task<ServiceResult<bool>> DeleteBorrowerAsync(int id)
        {
            var borrower = await _borrowerRepository.GetByIdAsync(id);
            if (borrower == null)
                return ServiceResult<bool>.Failure($"Borrower with ID {id} was not found.");

            _borrowerRepository.Delete(borrower);
            await _borrowerRepository.SaveChangesAsync();

            return ServiceResult<bool>.Success(true);
        }
        public async Task<ServiceResult<List<BorrowRecordDto>>> GetBorrowerHistoryAsync(int borrowerId)
        {
            var borrower = await _borrowerRepository.GetByIdAsync(borrowerId);
            if (borrower == null)
                return ServiceResult<List<BorrowRecordDto>>.Failure($"Borrower with ID {borrowerId} was not found.");

            var records = await _borrowerRepository.GetBorrowHistoryAsync(borrowerId);

            var dtos = records.Select(r => new BorrowRecordDto
            {
                Id = r.Id,
                BookId = r.BookId,
                BookTitle = r.Book?.Title ?? string.Empty,
                BorrowerId = r.BorrowerId,
                BorrowerName = r.Borrower?.Name ?? string.Empty,
                BorrowedAt = r.BorrowedAt,
                ReturnedAt = r.ReturnedAt,
                IsReturned = r.IsReturned
            }).ToList();
            return ServiceResult<List<BorrowRecordDto>>.Success(dtos);
        }
        private static BorrowerDto MapToBorrowerDto(Borrower borrower) => new BorrowerDto
        {
            Id = borrower.Id,
            Name = borrower.Name,
            Email = borrower.Email,
            PhoneNumber = borrower.PhoneNumber
        };
    }
}
