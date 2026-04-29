using LibraryManagement.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LibraryManagement.API.Filters
{
    public class ValidateIdParameterFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            foreach (var parameter in context.ActionDescriptor.Parameters)
            {
                if (parameter.ParameterType == typeof(int) && parameter.Name.ToLower().Contains("id"))
                {
                    var value = context.ActionArguments[parameter.Name];
                    if (value is int id && id <= 0)
                    {
                        context.Result = new BadRequestObjectResult(new ApiResponse<object>
                        {
                            Message = $"{parameter.Name} must be greater than zero."
                        });
                        return;
                    }
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
