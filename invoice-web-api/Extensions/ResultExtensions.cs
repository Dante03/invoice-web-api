using invoice_web_api.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace invoice_web_api.Extensions
{
    public static class ResultExtensions
    {
        public static IActionResult ToActionResult<T>(this Result<T> result)
        {
            if (!result.Success)
                return new BadRequestObjectResult(result.Error);

            return new OkObjectResult(result.Data);
        }
    }
}
