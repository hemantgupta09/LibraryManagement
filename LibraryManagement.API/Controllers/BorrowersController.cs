using LibraryManagement.API.Filters;
using LibraryManagement.API.Models;
using LibraryManagement.Application.DTO;
using LibraryManagement.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(ValidateModelStateFilter))]
    public class BorrowersController : BaseController
    {
        private readonly IBorrowerService _borrowerService;

        public BorrowersController(IBorrowerService borrowerService)
        {
            _borrowerService = borrowerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var borrowers = await _borrowerService.GetAllBorrowersAsync();
            return HandleResult(borrowers, "Borrowers retrieved successfully");
        }

        [HttpGet("{id:int}")]
        [ServiceFilter(typeof(ValidateIdParameterFilter))]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _borrowerService.GetBorrowerByIdAsync(id);
            return HandleResult(result, "Borrower retrieved successfully");
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBorrowerDto dto)
        {
            var result = await _borrowerService.CreateBorrowerAsync(dto);
            return HandleCreateResult(result, nameof(GetById), new { id = result.Data?.Id }, "Borrower created successfully");
        }

        [HttpPut("{id:int}")]
        [ServiceFilter(typeof(ValidateIdParameterFilter))]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateBorrowerDto dto)
        {
            var result = await _borrowerService.UpdateBorrowerAsync(id, dto);
            return HandleResult(result, "Borrower updated successfully");
        }

        [HttpDelete("{id:int}")]
        [ServiceFilter(typeof(ValidateIdParameterFilter))]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _borrowerService.DeleteBorrowerAsync(id);
            return HandleResult(result, "Borrower deleted successfully");
        }
        [HttpGet("{id:int}/history")]
        [ServiceFilter(typeof(ValidateIdParameterFilter))]
        public async Task<IActionResult> GetHistory(int id)
        {
            var result = await _borrowerService.GetBorrowerHistoryAsync(id);
            return HandleResult(result, "Borrower History retrieved successfully");
        }
    }
}
