using Microsoft.AspNetCore.Mvc;
using Test1.DTOs;
using Test1.Service;

namespace Test1.Controllers
{
    [ApiController]
    [Route("api/makers")]
    public class MakerController : ControllerBase
    {
        private readonly IMakerService _service;

        public MakerController(IMakerService service)
        {
            _service = service;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetMaker(int id)
        {
            var result = await _service.GetMaker(id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMaker([FromBody] CreateMakerRequest request)
        {
            var id = await _service.CreateMaker(request);

            return Created($"/api/makers/{id}", new { id });
        }
    }
}