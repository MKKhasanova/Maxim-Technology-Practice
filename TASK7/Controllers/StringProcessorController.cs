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
        private readonly IRequestLimiter _limiter;

        public StringProcessorController(
            IStringProcessorService service,
            IRequestLimiter limiter)
        {
            _service = service;
            _limiter = limiter;
        }

        [HttpGet("process")]
        public async Task<IActionResult> ProcessString([FromQuery] ProcessStringRequest request)
        {
            if (!_limiter.TryAcquireSlot())
            {
                return StatusCode(503, "Сервис перегружен. Попробуйте позже.");
            }

            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _service.ProcessString(request);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch
            {
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
            finally
            {
                _limiter.ReleaseSlot();
            }
        }
    }
}
