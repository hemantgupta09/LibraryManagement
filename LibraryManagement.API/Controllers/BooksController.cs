using LibraryManagement.API.Filters;
using LibraryManagement.API.Models;
using LibraryManagement.Application.DTO;
using LibraryManagement.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(ValidateModelStateFilter))]
    public class BooksController : BaseController
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var books = await _bookService.GetAllBooksAsync();
            return HandleResult(books, "Authors retrieved successfully");
        }
        [HttpGet("{id:int}")]
        [ServiceFilter(typeof(ValidateIdParameterFilter))]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _bookService.GetBookByIdAsync(id);
            return HandleResult(result, "Book retrieved successfully");
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBookDto dto)
        {
            var result = await _bookService.CreateBookAsync(dto);
            return HandleCreateResult(result, nameof(GetById), new { id = result.Data?.Id }, "Book created successfully");
        }
        [HttpPut("{id:int}")]
        [ServiceFilter(typeof(ValidateIdParameterFilter))]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateBookDto dto)
        {
            var result = await _bookService.UpdateBookAsync(id, dto);
            return HandleResult(result, "Book updated successfully");
        }
        [HttpDelete("{id:int}")]
        [ServiceFilter(typeof(ValidateIdParameterFilter))]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _bookService.DeleteBookAsync(id);
            return HandleResult(result, "Book deleted successfully");
        }
        [HttpPost("{id:int}/borrow")]
        [ServiceFilter(typeof(ValidateIdParameterFilter))]
        public async Task<IActionResult> Borrow(int id, [FromBody] BorrowBookDto dto)
        {
            var result = await _bookService.BorrowBookAsync(id, dto);
            return HandleResult(result, "Book borrowed successfully");
        }
        [HttpPost("{id:int}/return")]
        [ServiceFilter(typeof(ValidateIdParameterFilter))]
        public async Task<IActionResult> Return(int id)
        {
            var result = await _bookService.ReturnBookAsync(id);
            return HandleResult(result, "Book returned successfully");
        }
    }
}
