using LibraryManagement.API.Models;
using LibraryManagement.Application.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Controllers
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected IActionResult HandleResult<T>(ServiceResult<T> result, string successMessage = "Success")
        {
            if (!result.IsSuccess)
                return NotFound(new ApiResponse<object> { Message = result.ErrorMessage! });

            return Ok(new ApiResponse<T>
            {
                Data = result.Data,
                Message = successMessage
            });
        }

        protected IActionResult HandleCreateResult<T>(ServiceResult<T> result, string actionName, object routeValues, string successMessage = "Created")
        {
            if (!result.IsSuccess)
                return BadRequest(new ApiResponse<object> { Message = result.ErrorMessage! });

            return CreatedAtAction(actionName, routeValues, new ApiResponse<T>
            {
                Data = result.Data,
                Message = successMessage
            });
        }
    }
}
