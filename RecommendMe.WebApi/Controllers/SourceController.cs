using Microsoft.AspNetCore.Mvc;
using RecommendMe.Services.Abstract;
using RecommendMe.Services.Implementation;
using RecommendMe.Services.Mappers;

namespace RecommendMe.WebApi.Controllers
{
    public class SourceController : ControllerBase
    {
        private readonly ISourceService _sourceService;
        private readonly ILogger<SourceController> _logger;

        public SourceController(ISourceService sourceService)
        {
            _sourceService = sourceService;
        }

        [HttpGet("getsourcebyid/{id}")]
        public async Task<IActionResult> GetSourceById([FromRoute] int id)
        {
            var source = await _sourceService.GetByIdAsync(id);

            if (source == null)
            {
                return NotFound();
            }

            return Ok(source);
        }

        [HttpGet("getsources")]
        public async Task<IActionResult> GetSources(CancellationToken token = default)
        {
            try
            {
                var sorces = (await _sourceService.GetSourceWithRss(token));

                return Ok(sorces);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        [HttpPatch("updatesource/{id}")]
        public async Task<IActionResult> UpdateSourc()
        {
            return Ok();
        }

        [HttpDelete("deletesource/{id}")]
        public async Task<IActionResult> DeleteSourc()
        {
            return Ok();
        }
    }
}
