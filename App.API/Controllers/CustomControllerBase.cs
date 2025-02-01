using App.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace App.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomControllerBase : ControllerBase
    {
        [NonAction]
        public IActionResult CreateActionResult<T>(ServiceResult<T> result)
        {
            if(result.StatusCode == HttpStatusCode.NoContent)
            {
                return NoContent();
                // return new ObjectResult(null) { StatusCode = (int) result.StatusCode };
            }

            if(result.StatusCode == HttpStatusCode.Created)
            {
                return Created(result.UrlAsCreated, result);
            }

            return new ObjectResult(result) { StatusCode = (int) result.StatusCode };
        }

        [NonAction]
        public IActionResult CreateActionResult(ServiceResult result)
        {
            if(result.StatusCode == HttpStatusCode.NoContent)
            {
                return new ObjectResult(null) {StatusCode = (int) result.StatusCode };
            }

            return new ObjectResult(result) { StatusCode = (int) result.StatusCode };
        }
    }
}
