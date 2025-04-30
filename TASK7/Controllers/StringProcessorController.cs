using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using StringProcessor.API.Models.Requests;
using StringProcessor.API.Services.Interfaces;

namespace StringProcessor.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StringProcessorController : ControllerBase
    {
        private readonly IStringProcessorService _service;

        public StringProcessorController(IStringProcessorService service)
        {
            _service = service;
        }

        [HttpGet("process")]
        public async Task<IActionResult> ProcessString([FromQuery] ProcessStringRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var result = await _service.ProcessString(request);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch
            {
                return StatusCode(500, "Произошла внутренняя ошибка сервера");
            }
        }
    }
}
