using DATEX_ProjectDatabase.Interfaces;
using DATEX_ProjectDatabase.Model;
using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DATEX_ProjectDatabase.Service
{
    public class VocAnalysisService
    {
        private readonly IVocAnalysisRepository _vocAnalysisRepository;

        public VocAnalysisService(IVocAnalysisRepository vocAnalysisRepository)
        {
            _vocAnalysisRepository = vocAnalysisRepository;
        }

        public async Task ProcessExcelFileAsync(Stream fileStream)
        {
            var vocAnalyses = new List<VOCAnalysis>();

            using (var package = new ExcelPackage(fileStream))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                int rowCount = worksheet.Dimension.Rows;
                int colCount = worksheet.Dimension.Columns;

                // Column indices based on provided headers
                var columnIndices = new Dictionary<string, int[]>
                {
                    { "CustomerFocus", new[] { 8, 10, 12 } }, // Columns 8, 10, 12 for CustomerFocus
                    { "PlanningAndControl", new[] { 14, 16 } }, // Columns 14, 16 for PlanningAndControl
                    { "Quality", new[] { 18 } }, // Column 18 for Quality
                    { "Communication", new[] { 20, 22 } }, // Columns 20, 22 for Communication
                    { "Knowledge", new[] { 24 } }, // Column 24 for Knowledge
                    { "EngageService", new[] { 26 } }, // Column 26 for EngageService
                    { "Score", new[] { 28 } } // Column 28 for Score
                };

                // Log headers for debugging
                for (int col = 1; col <= colCount; col++)
                {
                    var headerValue = worksheet.Cells[2, col].Value?.ToString().Trim();
                    System.Diagnostics.Debug.WriteLine($"Column {col}: {headerValue}");
                }

                // Check if required columns exist and are not empty
                foreach (var column in columnIndices)
                {
                    foreach (var index in column.Value)
                    {
                        if (index > colCount)
                        {
                            throw new Exception($"Required column with index {index} is missing in the Excel file.");
                        }

                        var headerValue = worksheet.Cells[2, index].Value?.ToString().Trim();
                        if (string.IsNullOrEmpty(headerValue))
                        {
                            throw new Exception($"Required header with index {index} is missing or empty.");
                        }
                    }
                }

                // Read data rows
                for (int row = 3; row <= rowCount; row++)
                {
                    // Process each column for CustomerFocus
                    foreach (var col in columnIndices["CustomerFocus"])
                    {
                        var value = worksheet.Cells[row, col].Value?.ToString().Trim();
                        if (!string.IsNullOrEmpty(value))
                        {
                            vocAnalyses.Add(new VOCAnalysis
                            {
                                CustomerFocus = value,
                                // Set other properties as needed
                                PlanningAndControl = "", // Placeholder if needed
                                Quality = "", // Placeholder if needed
                                Communication = "", // Placeholder if needed
                                Knowledge = "", // Placeholder if needed
                                EngageService = "", // Placeholder if needed
                                Score = 0 // Placeholder if needed
                            });
                        }
                    }

                    // Process each column for PlanningAndControl
                    foreach (var col in columnIndices["PlanningAndControl"])
                    {
                        var value = worksheet.Cells[row, col].Value?.ToString().Trim();
                        if (!string.IsNullOrEmpty(value))
                        {
                            vocAnalyses.Add(new VOCAnalysis
                            {
                                PlanningAndControl = value,
                                // Set other properties as needed
                                CustomerFocus = "", // Placeholder if needed
                                Quality = "", // Placeholder if needed
                                Communication = "", // Placeholder if needed
                                Knowledge = "", // Placeholder if needed
                                EngageService = "", // Placeholder if needed
                                Score = 0 // Placeholder if needed
                            });
                        }
                    }

                    // Process Quality column
                    var qualityValue = worksheet.Cells[row, columnIndices["Quality"][0]].Value?.ToString().Trim();
                    if (!string.IsNullOrEmpty(qualityValue))
                    {
                        vocAnalyses.Add(new VOCAnalysis
                        {
                            Quality = qualityValue,
                            // Set other properties as needed
                            CustomerFocus = "", // Placeholder if needed
                            PlanningAndControl = "", // Placeholder if needed
                            Communication = "", // Placeholder if needed
                            Knowledge = "", // Placeholder if needed
                            EngageService = "", // Placeholder if needed
                            Score = 0 // Placeholder if needed
                        });
                    }

                    // Process Communication columns
                    foreach (var col in columnIndices["Communication"])
                    {
                        var value = worksheet.Cells[row, col].Value?.ToString().Trim();
                        if (!string.IsNullOrEmpty(value))
                        {
                            vocAnalyses.Add(new VOCAnalysis
                            {
                                Communication = value,
                                // Set other properties as needed
                                CustomerFocus = "", // Placeholder if needed
                                PlanningAndControl = "", // Placeholder if needed
                                Quality = "", // Placeholder if needed
                                Knowledge = "", // Placeholder if needed
                                EngageService = "", // Placeholder if needed
                                Score = 0 // Placeholder if needed
                            });
                        }
                    }

                    // Process Knowledge column
                    var knowledgeValue = worksheet.Cells[row, columnIndices["Knowledge"][0]].Value?.ToString().Trim();
                    if (!string.IsNullOrEmpty(knowledgeValue))
                    {
                        vocAnalyses.Add(new VOCAnalysis
                        {
                            Knowledge = knowledgeValue,
                            // Set other properties as needed
                            CustomerFocus = "", // Placeholder if needed
                            PlanningAndControl = "", // Placeholder if needed
                            Quality = "", // Placeholder if needed
                            Communication = "", // Placeholder if needed
                            EngageService = "", // Placeholder if needed
                            Score = 0 // Placeholder if needed
                        });
                    }

                    // Process EngageService column
                    var engageServiceValue = worksheet.Cells[row, columnIndices["EngageService"][0]].Value?.ToString().Trim();
                    if (!string.IsNullOrEmpty(engageServiceValue))
                    {
                        vocAnalyses.Add(new VOCAnalysis
                        {
                            EngageService = engageServiceValue,
                            // Set other properties as needed
                            CustomerFocus = "", // Placeholder if needed
                            PlanningAndControl = "", // Placeholder if needed
                            Quality = "", // Placeholder if needed
                            Communication = "", // Placeholder if needed
                            Knowledge = "", // Placeholder if needed
                            Score = 0 // Placeholder if needed
                        });
                    }

                    // Process Score column
                    var scoreValue = worksheet.Cells[row, columnIndices["Score"][0]].Value?.ToString().Trim();
                    if (int.TryParse(scoreValue, out int score))
                    {
                        vocAnalyses.Add(new VOCAnalysis
                        {
                            Score = score,
                            // Set other properties as needed
                            CustomerFocus = "", // Placeholder if needed
                            PlanningAndControl = "", // Placeholder if needed
                            Quality = "", // Placeholder if needed
                            Communication = "", // Placeholder if needed
                            Knowledge = "", // Placeholder if needed
                            EngageService = "" // Placeholder if needed
                        });
                    }
                }
            }

            await _vocAnalysisRepository.SaveVocAnalysesAsync(vocAnalyses);
        }
    }
}
