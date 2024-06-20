using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

/// <summary>
/// A controller responsible for handling errors and exceptions that can't be processed.
/// </summary>
public class ErrorsController : ControllerBase
{
    /// <summary>
    /// Handles exceptions and returns a problem details response.
    /// </summary>
    [ApiExplorerSettings(IgnoreApi = true)] // swagger ignore this controller method
    [Route("/error")]
    public IActionResult Error()
    {
        var exception = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;

        return Problem();
    }
}