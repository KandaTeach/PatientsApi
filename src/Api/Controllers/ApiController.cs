using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Api.Controllers;

/// <summary>
/// Base controller class for API controllers.
/// </summary>
[ApiController]
public class ApiController : ControllerBase
{
    /// <summary>
    /// Handles error responses by returning appropriate HTTP status codes and messages.
    /// </summary>
    /// <param name="errors">A list of errors to be processed.</param>
    /// <returns>Return a proper HTTP code response with error messages.</returns>
    protected IActionResult Problem(List<Error> errors)
    {
        // for validation errors
        if (errors.All(error => error.Type == ErrorType.Validation))
        {
            var modelStateDictionary = new ModelStateDictionary();

            foreach (var error in errors)
            {
                modelStateDictionary.AddModelError(
                    error.Code,
                    error.Description
                );
            }

            return ValidationProblem(modelStateDictionary);
        }

        HttpContext.Items["errors"] = errors;

        var firstError = errors[0];

        var statusCode = firstError.Type switch
        {
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.Failure => StatusCodes.Status500InternalServerError,
            ErrorType.Unexpected => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status500InternalServerError,
        };

        return Problem(statusCode: statusCode, title: firstError.Description);
    }
}