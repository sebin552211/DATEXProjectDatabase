using DATEX_ProjectDatabase.Model;
using DATEX_ProjectDatabase.Models;
using Microsoft.AspNetCore.Mvc;

namespace DATEX_ProjectDatabase.Interfaces
{
    public interface IVocAnalysisRepository
    {
        Task<List<VOCAnalysis>> GetAllVocAnalysesAsync();
     
        Task SaveVocAnalysesAsync(IEnumerable<VOCAnalysis> vocAnalyses);
        /*Task<VOCAnalysis> SurveyIdExistsAsync(string surveyId);*/
        /*Task AddAsync(VOCAnalysis vocAnalysis);*/
        Task SaveVocAnalysesAsync(List<VOCAnalysis> vocAnalyses);
        Task<HashSet<string>> GetExistingResponseIdsAsync();
        Task UpdateVocAnalysisAsync(VOCAnalysis vocAnalysis);
        Task<List<VOCAnalysis>> GetFeedbackByDUandSurveyId(string Du = null, string SurveyId = null);
        Task<VOCAnalysis> GetBySurveyIdAsync(string surveyId);
        Task<List<VOCAnalysis>> GetBySurveyIdsAsync(List<string> surveyIds);
        Task UpdateVOCAnalysisAsync(VOCAnalysis vocAnalysis);
        Task<IEnumerable<VOCAnalysis>> GetFilteredFeedbackAsyncBySurveyId(string surveyId);
        Task<List<VOCAnalysis>> GetBySurveyIdAsyncforSatisfactory_Score(List<string> surveyId);
        Task<List<VOCAnalysis>> GetSurveysByQuarterAsync(string quarter);
        Task<VOCAnalysis> AddDUinSurveyId(string Du, string SurveyId);
        Task<VOCAnalysis> DeleteDUinSurveyId(string SurveyId);
        Task<List<VOCAnalysis>> GetFeedbackByDU(string Du);

    }
}
