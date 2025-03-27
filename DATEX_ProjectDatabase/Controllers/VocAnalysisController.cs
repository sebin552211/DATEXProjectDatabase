using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;
using DATEX_ProjectDatabase.Service;
using DATEX_ProjectDatabase.Interfaces;
using DATEX_ProjectDatabase.Models;
using DATEX_ProjectDatabase.Model;
using Microsoft.EntityFrameworkCore;
using Azure;
using System.Transactions;
using OfficeOpenXml;
using System.Text.RegularExpressions;

namespace DATEX_ProjectDatabase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VocAnalysisController : ControllerBase
    {
        private readonly VocAnalysisService _vocAnalysisService;
        private readonly IVocAnalysisRepository _projectRepository;
        private readonly IProjectRepository _projectsRepository;

        public VocAnalysisController(VocAnalysisService vocAnalysisService, IVocAnalysisRepository vocAnalysisRepository, IProjectRepository projectRepository)
        {
            _vocAnalysisService = vocAnalysisService;
            _projectRepository = vocAnalysisRepository;
            _projectsRepository = projectRepository;
        }

        [HttpPost("Post/DU")]
        public async Task<IActionResult> AddDU(string Du, string SurveyId)
        {
            if (SurveyId==null || Du==null)
            {
                return BadRequest("Please provide valid SurveyId or DU name");
            }
            var feedback = await _projectRepository.AddDUinSurveyId(Du, SurveyId);
            if (feedback == null)
            {
                return BadRequest("not found");
            }
            return Ok(feedback);
        }

        [HttpDelete("Delete/DU")]
        public async Task<IActionResult> DeleteDU(string SurveyId) 
        {
            if (string.IsNullOrEmpty(SurveyId))
            {
                return BadRequest("Survey Id not Found");
            }
            var feedback = await _projectRepository.DeleteDUinSurveyId(SurveyId);
            if (feedback == null)
            {
                return BadRequest("not found");
            }
            return Ok();
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
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var filePath = Path.Combine(uploadsFolder, file.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                FileStorage.UploadedFilePath = filePath;

                using (var stream = file.OpenReadStream())
            {
                await _vocAnalysisService.ProcessExcelFileAsync(stream);
            }

            return Ok("File processed successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("surveyId")]
        public async Task<IActionResult> GetSurveyId()
        {
            try
            {
                var vocAnalyses = await _projectRepository.GetAllVocAnalysesAsync();
                var surveyIds = vocAnalyses
                                     .Select(v => v.SurveyId)
                                     .ToList();
                return Ok(surveyIds);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error.");
            }
        }
        [HttpGet("DU")]
        public async Task<IActionResult> GetAllDUs()
        {
            try
            {
                var vocAnalyses = await _projectRepository.GetAllVocAnalysesAsync();
                var surveyIds = vocAnalyses
                                     .Select(v => v.DU)
                                     .Distinct()
                                     .ToList();
                return Ok(surveyIds);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error.");
            }

        }

        [HttpGet]
        public async Task<IActionResult> GetFeedback(string Du = null, string SurveyId = null, string Quarter = null)
        {
            try
            {
                if (string.IsNullOrEmpty(Quarter) && string.IsNullOrEmpty(Du) && string.IsNullOrEmpty(SurveyId))
                {
                    var vocAnalyses = await _projectRepository.GetAllVocAnalysesAsync();
                    return Ok(vocAnalyses);
                }

                var feedback = await _projectRepository.GetFeedbackByDUandSurveyId(Du, SurveyId);
                if (feedback == null || !feedback.Any())
                {
                    return NotFound("No feedback found.");
                }

                if (!string.IsNullOrEmpty(Quarter))
                {
                    var dateRange = GetQuarterDateRange(Quarter);
                    if (dateRange == null)
                    {
                        return BadRequest("Invalid Quarter format.");
                    }

                    var (startDate, endDate) = dateRange.Value;
                    feedback = feedback.Where(p => p.Response_Completion_Time >= startDate && p.Response_Completion_Time <= endDate).ToList();
                }

                return Ok(feedback);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



        private (DateTime startDate, DateTime endDate)? GetQuarterDateRange(string quarter)
        {
            
            var match = Regex.Match(quarter, @"Q([1-4]) \((\d{4})-(\d{4})\)");
            if (!match.Success)
            {
                return null;
            }

            int quarterNumber = int.Parse(match.Groups[1].Value);
            int startYear = int.Parse(match.Groups[2].Value);
            int endYear = int.Parse(match.Groups[3].Value);

            DateTime startDate, endDate;

            switch (quarterNumber)
            {
                case 1:
                    startDate = new DateTime(startYear, 4, 1); // April 1st
                    endDate = new DateTime(startYear, 6, 30);  // June 30th
                    break;
                case 2:
                    startDate = new DateTime(startYear, 7, 1); // July 1st
                    endDate = new DateTime(startYear, 9, 30);  // September 30th
                    break;
                case 3:
                    startDate = new DateTime(startYear, 10, 1); // October 1st
                    endDate = new DateTime(startYear, 12, 31);    // December 31st
                    break;
                case 4:
                    startDate = new DateTime(endYear, 1, 1);  // January 1st
                    endDate = new DateTime(endYear, 3, 31);   // March 31st
                    break;
                default:
                    return null;
            }

            return (startDate, endDate);
        }

        [HttpGet("calculate/SurveyId")]
        public async Task<IActionResult> CalculateSatisfactoryScore(string Quarter = null, string SurveyId = null)
        {
            try
            {
                List<VOCAnalysis> surveysInQuarter = new List<VOCAnalysis>();

                if (!string.IsNullOrEmpty(Quarter))
                {
                    surveysInQuarter = await _projectRepository.GetSurveysByQuarterAsync(Quarter);
                }

                if (!string.IsNullOrEmpty(SurveyId))
                {
                    if (surveysInQuarter.Any())
                    {
                        surveysInQuarter = surveysInQuarter.Where(s => s.SurveyId == SurveyId).ToList();
                    }

                    if (!surveysInQuarter.Any()) 
                    {
                        var survey = await _projectRepository.GetBySurveyIdAsync(SurveyId);
                        if (survey != null && (string.IsNullOrEmpty(Quarter) || GetQuarterFromDate(survey.Response_Completion_Time) == Quarter))
                        {
                            surveysInQuarter.Add(survey);
                        }
                    }

                    if (!surveysInQuarter.Any())
                    {
                        return NotFound("No matching Survey Id found in the given quarter.");
                    }
                }

                if (!surveysInQuarter.Any())
                {
                    return NotFound("No surveys found for the given quarter.");
                }

                int totalNA = 0;
                double totalScore = 0;
                int surveyCount = surveysInQuarter.Count();

                var surveyIds = surveysInQuarter.Select(s => s.SurveyId).ToList();

                var vocAnalyses = await _projectRepository.GetBySurveyIdAsyncforSatisfactory_Score(surveyIds);
                var vocAnalysisData = await _projectRepository.GetBySurveyIdsAsync(surveyIds);

                var groupedNAValues = vocAnalyses
                    .Where(v => surveyIds.Contains(v.SurveyId) && (string.IsNullOrEmpty(Quarter) || GetQuarterFromDate(v.Response_Completion_Time) == Quarter))
                    .GroupBy(v => v.SurveyId)
                    .Select(group => new
                    {
                        SurveyId = group.Key,
                        NACount = group.Sum(v => CountNAValuesInFeedback(v))
                    })
                    .ToList();

                totalNA = groupedNAValues.Sum(g => g.NACount);

                var groupedScores = vocAnalysisData
                    .Where(v => surveyIds.Contains(v.SurveyId) && (string.IsNullOrEmpty(Quarter) || GetQuarterFromDate(v.Response_Completion_Time) == Quarter))
                    .GroupBy(v => v.SurveyId)
                    .Select(group => new
                    {
                        SurveyId = group.Key,
                        TotalScore = group.Sum(v => v.Score)
                    })
                    .ToList();

                totalScore = groupedScores.Sum(g => g.TotalScore);

                double denominator = (surveyCount * 48) - (totalNA * 5);
                if (denominator <= 0) denominator = 1;

                double satisfactoryScore = (totalScore / denominator) * 100;

                return Ok(new
                {
                    satisfactory_score = Math.Floor(satisfactoryScore),
                    totalScore,
                    denominator,
                    surveysInQuarter,
                    totalNA,
                    surveyCount
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        private string GetQuarterFromDate(DateTime? responseTime)
        {
            if (!responseTime.HasValue)
            {
                return "Unknown"; // Handle null case
            }

            int year = responseTime.Value.Year;
            int month = responseTime.Value.Month;

            string quarter;
            if (month >= 4 && month <= 6)
                quarter = $"Q1 ({year}-{year + 1})";
            else if (month >= 7 && month <= 9)
                quarter = $"Q2 ({year}-{year + 1})";
            else if (month >= 10 && month <= 12)
                quarter = $"Q3 ({year}-{year + 1})";
            else
                quarter = $"Q4 ({year - 1}-{year})"; // Jan-March falls in previous financial year

            return quarter;
        }




        private int CountNAValuesInFeedback(VOCAnalysis voc)
        {
            int naCount = 0;

            // Ensure lists exist before checking them
            if (voc.CustomerFocus != null && voc.CustomerFocus.Any())
            {
                naCount += voc.CustomerFocus.Count(feedback => feedback == "N/A");
            }

            if (voc.PlanningAndControl != null && voc.PlanningAndControl.Any())
            {
                naCount += voc.PlanningAndControl.Count(feedback => feedback == "N/A");
            }

            if (voc.Communication != null && voc.Communication.Any())
            {
                naCount += voc.Communication.Count(feedback => feedback == "N/A");
            }

            if (!string.IsNullOrEmpty(voc.Quality))
            {
                string[] qualityList = voc.Quality.Split(',');
                naCount += qualityList.Count(feedback => feedback.Trim() == "N/A");
            }

            if (!string.IsNullOrEmpty(voc.Quality))
            {
                string[] qualityList = voc.Knowledge.Split(','); // Adjust delimiter if needed
                naCount += qualityList.Count(feedback => feedback.Trim() == "N/A");
            }

            return naCount;
        }



        [HttpGet("calculate")]
        public async Task<IActionResult> CalculateSatisfactoryScore()
        {

            try
            {
                // Fetch all records from database
                var vocAnalyses = await _projectRepository.GetAllVocAnalysesAsync();

                if (vocAnalyses == null || !vocAnalyses.Any())
                {
                    return NotFound("No data available for calculation.");
                }

                double totalScore = 0;
                int totalRows = 0;

                foreach (var voc in vocAnalyses)
                {
                    if (voc.Score > 0) // Ensure score is valid
                    {
                        totalScore += voc.Score;
                        totalRows++;
                    }
                }

                double Score = (totalScore / totalRows);
                double satisfactoryScore = (Score / 48) * 100;

                return Ok(new { satisfactory_score = Math.Floor(satisfactoryScore) });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("get-satisfaction-counts")]
        public IActionResult GetSatisfactionCounts()
        {
            var filePath = FileStorage.UploadedFilePath;

            if (string.IsNullOrEmpty(filePath) || !System.IO.File.Exists(filePath))
            {
                return NotFound("No uploaded file found or the file does not exist.");
            }

            try
            {
                using (var package = new OfficeOpenXml.ExcelPackage(new FileInfo(filePath)))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    int rowCount = worksheet.Dimension.Rows;

                    int verySatisfied = 0, satisfied = 0, neitherSatisfiedNorDissatisfied = 0, dissatisfied = 0;

                    for (int row = 3; row <= rowCount; row++)
                    {
                        for (int col = 9; col <= 25; col+=2)
                        {
                            var satisfactionValue = worksheet.Cells[row, col].Text.Trim(); 
                            switch (satisfactionValue)
                            {
                                case "Very Satisfied":
                                    verySatisfied++;
                                    break;
                                case "Satisfied":
                                    satisfied++;
                                    break;
                                case "Neither Satisfied nor Dissatisfied":
                                    neitherSatisfiedNorDissatisfied++;
                                    break;
                                case "Dissatisfied":
                                    dissatisfied++;
                                    break;
                            }
                        }
                    }

                    // Return counts as a JSON response
                    return Ok(new
                    {
                        VerySatisfied = verySatisfied,
                        Satisfied = satisfied,
                        NeitherSatisfiedNorDisatisfied = neitherSatisfiedNorDissatisfied,
                        Disatisfied = dissatisfied
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing the file: " + ex.Message);
            }
        }

        [HttpGet("get-satisfaction-category-counts")]
        public IActionResult GetSatisfactionCategoryCounts()
        {
            // Get the uploaded file path from the static variable
            var filePath = FileStorage.UploadedFilePath;

            if (string.IsNullOrEmpty(filePath) || !System.IO.File.Exists(filePath))
            {
                return NotFound("No uploaded file found or the file does not exist.");
            }

            try
            {
                // Define category column mappings
                var categoryColumns = new Dictionary<string, List<int>>
                {
                    { "CustomerFocus", new List<int> { 9, 11, 13 } },
                    { "PlanningAndControl", new List<int> { 15, 17 } },
                    { "Quality", new List<int> { 19 } },
                    { "Communication", new List<int> { 21, 23 } },
                    { "Knowledge", new List<int> { 25 } },
                };  

                // Open the Excel file using EPPlus
                using (var package = new OfficeOpenXml.ExcelPackage(new FileInfo(filePath)))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    int rowCount = worksheet.Dimension.Rows;
                    var categoryCounts = new Dictionary<string, Dictionary<string, int>>();
                    foreach (var category in categoryColumns.Keys)
                    {
                        categoryCounts[category] = new Dictionary<string, int>
                        {
                            { "Very Satisfied", 0 },
                            { "Satisfied", 0 },
                            { "Neither Satisfied nor Dissatisfied", 0 },
                            { "Dissatisfied", 0 }
                        };
                    }

                    for (int row = 3; row <= rowCount; row++) 
                    {
                        foreach (var category in categoryColumns)
                        {
                            var categoryName = category.Key;
                            var columns = category.Value;

                            foreach (var col in columns)
                            {
                                var satisfactionValue = worksheet.Cells[row, col].Text.Trim(); 
                                if (categoryCounts[categoryName].ContainsKey(satisfactionValue))
                                {
                                    categoryCounts[categoryName][satisfactionValue]++;
                                }
                            }
                        }
                    }

                    // Return the counts for each category as a JSON response
                    return Ok(categoryCounts);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing the file: " + ex.Message);
            }
        }

    }
}