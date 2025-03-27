using DATEX_ProjectDatabase.Interfaces;
using DATEX_ProjectDatabase.Model;
using DATEX_ProjectDatabase.Repository;
using OfficeOpenXml;

namespace DATEX_ProjectDatabase.Service
{
    public class VocAnalysisService
    {
        private readonly IVocAnalysisRepository _vocAnalysisRepository;

        public VocAnalysisService(IVocAnalysisRepository vocAnalysisRepository)
        {
            _vocAnalysisRepository = vocAnalysisRepository;
        }

        public async Task<ProcessResult> ProcessExcelFileAsync(Stream fileStream)
        {
            var vocAnalyses = new List<VOCAnalysis>();

            using (var package = new ExcelPackage(fileStream))
            {
                var worksheet = package.Workbook.Worksheets[0];
                var rowCount = worksheet.Dimension.Rows;

                var existingResponseIds = await _vocAnalysisRepository.GetExistingResponseIdsAsync();

                for (int row = 3; row <= rowCount; row++) //row
                {
                    var surveyId = worksheet.Cells[row, 7].Text?.Trim(); //15 7
                    if (string.IsNullOrEmpty(surveyId)) continue;

                    var responseId = ReadCell(worksheet, row, new[] { 2 });

                    if (existingResponseIds.Contains(responseId)) continue;


                    // Add the record to the list without checking for duplicates
                    var newRecord = new VOCAnalysis
                    {
                        ResponseId = responseId,
                        SurveyId = surveyId,
                        CustomerFocus = ReadCell2(worksheet, row, new[] { 9, 11, 13 }), // 16, 20, 24 8, 10, 12
                        PlanningAndControl = ReadCell2(worksheet, row, new[] { 15, 17 }), // 28, 32 14, 16 
                        Quality = ReadCell(worksheet, row, new[] { 19 }), // 36 18
                        Communication = ReadCell2(worksheet, row, new[] { 21, 23 }), // 40, 44 20, 22
                        Knowledge = ReadCell(worksheet, row, new[] { 25 }), // 48 24
                        EngageService = ReadCell(worksheet, row, new[] { 27 }), // 52 26
                        Score = ReadScore(worksheet, row, 29), // 12 28
                        DU = ReadCell(worksheet, row , new[] { 8 }),
                        Response_Completion_Time = ReadCellAsDateTime(worksheet, row, new[] { 4 }) ?? default(DateTime), // 6 4
                    };

                    vocAnalyses.Add(newRecord);
                }
            }

            // Save all records to the database
            if (vocAnalyses.Any())
            {
                await _vocAnalysisRepository.SaveVocAnalysesAsync(vocAnalyses);
            }

            return new ProcessResult
            {
                NewDataCount = vocAnalyses.Count,
            };
        }


        private string ReadCell(ExcelWorksheet worksheet, int row, int[] columns)
        {
            foreach (var col in columns)
            {
                var value = worksheet.Cells[row, col].Value?.ToString().Trim();
                if (!string.IsNullOrEmpty(value))
                {
                    return value;
                }
            }
            return null;
        }

        private DateTime? ReadCellAsDateTime(ExcelWorksheet worksheet, int row, int[] columns)
        {
            foreach (var col in columns)
            {
                var value = worksheet.Cells[row, col].Value?.ToString().Trim();
                if (!string.IsNullOrEmpty(value) && DateTime.TryParse(value, out DateTime dateTime))
                {
                    return dateTime;
                }
            }
            return null;
        }



        private List<string> ReadCell2(ExcelWorksheet worksheet, int row, int[] columns)
        {
            var values = new List<string>();
            foreach (var col in columns)
            {
                var value = worksheet.Cells[row, col].Value?.ToString().Trim();
                if (!string.IsNullOrEmpty(value))
                {
                    values.Add(value);
                }
            }
            return values;
        }

        private int ReadScore(ExcelWorksheet worksheet, int row, int col)
        {
            var value = worksheet.Cells[row, col].Value?.ToString().Trim();
            return int.TryParse(value, out int score) ? score : 0;
        }

       /* public async Task<List<VOCAnalysis>> ProcessExcelFileAsync2(Stream fileStream)
        {
            var vocAnalyses = new List<VOCAnalysis>();

            using (var package = new ExcelPackage(fileStream))
            {
                var worksheet = package.Workbook.Worksheets[0]; 
                var rowCount = worksheet.Dimension.Rows;

                for (int row = 3; row <= rowCount; row++) 
                {
                    var surveyId = worksheet.Cells[row, 7].Text?.Trim(); // 15
                    if (!DateTime.TryParse(worksheet.Cells[row, 4].Text?.Trim(), out var responseCompletionTime))
                        continue;
                    var vocAnalysis = new VOCAnalysis
                    {
                        SurveyId = surveyId,
                        CustomerFocus = ReadCell2(worksheet, row, new[] { 8, 10, 12 }), // 16, 20, 24
                        PlanningAndControl = ReadCell2(worksheet, row, new[] { 14, 16 }), // 28, 32
                        Quality = ReadCell(worksheet, row, new[] { 18 }),  // 36
                        Communication = ReadCell2(worksheet, row, new[] { 20, 22 }),  // 40, 44
                        Knowledge = ReadCell(worksheet, row, new[] { 24 }), // 48
                        EngageService = ReadCell(worksheet, row, new[] { 26 }), // 52 
                        Score = ReadScore(worksheet, row, 28), // 12
                        Response_Completion_Time = ReadCellAsDateTime(worksheet, row, new[] { 4 }), // 6
                    };

                    vocAnalyses.Add(vocAnalysis);
                }
            }

            return vocAnalyses;
        }
*/
    }
}
public class ProcessResult
{
    public List<string> NewSurveyIds { get; set; } = new List<string>();
    public int NewDataCount { get; set; }
}
