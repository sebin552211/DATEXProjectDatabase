using DATEX_ProjectDatabase.Data;
using DATEX_ProjectDatabase.Interfaces;
using DATEX_ProjectDatabase.Model;
using Microsoft.EntityFrameworkCore;

namespace DATEX_ProjectDatabase.Repository
{
    public class VocAnalysisRepository
    : IVocAnalysisRepository
    {
        private readonly ApplicationDbContext _context;

        public VocAnalysisRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<VOCAnalysis>> GetAllVocAnalysesAsync()
        {
            return await _context.Set<VOCAnalysis>().ToListAsync(); // Replace Set<VOCAnalysis>() with your actual DbSet property
        }

        public async Task SaveVocAnalysesAsync(IEnumerable<VOCAnalysis> vocAnalyses)
        {
            _context.VocAnalyses.AddRange(vocAnalyses);
            await _context.SaveChangesAsync();
        }
    }
}
