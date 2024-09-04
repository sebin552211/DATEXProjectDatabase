using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;
using DATEX_ProjectDatabase.Service;
using DATEX_ProjectDatabase.Interfaces;

namespace DATEX_ProjectDatabase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VocAnalysisController : ControllerBase
    {
        private readonly VocAnalysisService _vocAnalysisService;
        private readonly IVocAnalysisRepository _vocAnalysisRepository;

        public VocAnalysisController(VocAnalysisService vocAnalysisService, IVocAnalysisRepository vocAnalysisRepository)
        {
            _vocAnalysisService = vocAnalysisService;
            _vocAnalysisRepository = vocAnalysisRepository;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            try
            {
                using (var stream = file.OpenReadStream())
                {
                    // Process the Excel file directly from the stream
                    await _vocAnalysisService.ProcessExcelFileAsync(stream);
                }

                return Ok("File processed successfully.");
            }
            catch (Exception ex)
            {
                // Log the exception and return a failure response
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var vocAnalyses = await _vocAnalysisRepository.GetAllVocAnalysesAsync();
                return Ok(vocAnalyses);
            }
            catch (Exception ex)
            {
                // Log the exception (consider using a logging framework)
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
