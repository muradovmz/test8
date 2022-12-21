using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using {{cookiecutter.ProjectName}}.Framework.Shared.Exception;

namespace {{cookiecutter.ProjectName}}.Framework.Api.Filter
{
    /// <summary>
    /// Retrieve all validation errors triggered by fluent validations 
    /// </summary>
    public class ModelValidationFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            { 
                var modelState = context.ModelState;
                var uniqueFieldErrors =  modelState.Keys.SelectMany(key => modelState[key]?
                    .Errors.Select(x => x.ErrorMessage)).ToList();
                throw new BadRequestException(string.Join("",uniqueFieldErrors));
            }
            await next();
        }
    } 
}