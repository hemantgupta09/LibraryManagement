using LibraryManagement.API.Filters;
using LibraryManagement.Application.DTO;
using LibraryManagement.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(ValidateModelStateFilter))]
    public class AuthorsController : BaseController
    {
        private readonly IAuthorService _authorService;
        public AuthorsController(IAuthorService authorService)
        {
            _authorService = authorService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _authorService.GetAllAuthorsAsync();
            return HandleResult(result, "Authors retrieved successfully");
        }
        [HttpGet("{id:int}")]
        [ServiceFilter(typeof(ValidateIdParameterFilter))]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _authorService.GetAuthorByIdAsync(id);
            return HandleResult(result, "Author retrieved successfully");
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAuthorDto dto)
        {
            var result = await _authorService.CreateAuthorAsync(dto);
            return HandleCreateResult(result, nameof(GetById), new { id = result.Data!.Id },
                "Author created successfully");
        }
        [HttpPut("{id:int}")]
        [ServiceFilter(typeof(ValidateIdParameterFilter))]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateAuthorDto dto)
        {
            var result = await _authorService.UpdateAuthorAsync(id, dto);
            return HandleResult(result, "Author updated successfully");
        }
        [HttpDelete("{id:int}")]
        [ServiceFilter(typeof(ValidateIdParameterFilter))]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _authorService.DeleteAuthorAsync(id);
            return HandleResult(result, "Author deleted successfully");
        }
    }
}
