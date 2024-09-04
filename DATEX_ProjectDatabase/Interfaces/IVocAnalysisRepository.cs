using DATEX_ProjectDatabase.Model;

namespace DATEX_ProjectDatabase.Interfaces
{
    public interface IVocAnalysisRepository
    {
        Task<List<VOCAnalysis>> GetAllVocAnalysesAsync();
     
        Task SaveVocAnalysesAsync(IEnumerable<VOCAnalysis> vocAnalyses);
    }
}
