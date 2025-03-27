using DATEX_ProjectDatabase.Data;
using DATEX_ProjectDatabase.Interfaces;
using DATEX_ProjectDatabase.Migrations;
using DATEX_ProjectDatabase.Model;
using DATEX_ProjectDatabase.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace DATEX_ProjectDatabase.Repository
{
    public class VocAnalysisRepository: IVocAnalysisRepository
    {
        private readonly ApplicationDbContext _context;

        public VocAnalysisRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<VOCAnalysis>> GetAllVocAnalysesAsync()
        {
            return await _context.Set<VOCAnalysis>().ToListAsync(); 
        }

        public async Task<VOCAnalysis> AddDUinSurveyId(string Du, string SurveyId)
        {
            var feedback = await GetBySurveyIdAsync(SurveyId);
            if (feedback == null)
            {
                return null;
            }
            feedback.DU = Du;
            await _context.SaveChangesAsync();
            return feedback;
        }

        /*   public async Task<VOCAnalysis> UpdateDUinSurveyId(string Du, string SurveyId)
           {
               var feedback = await GetBySurveyIdAsync(SurveyId);
               if (feedback == null)
               {
                   return null;
               }
               feedback.DU = Du;
               await _context.SaveChangesAsync();
               return feedback;
           }*/

        public async Task<HashSet<string>> GetExistingResponseIdsAsync()
        {
            return new HashSet<string>(await _context.VocAnalyses.Select(v => v.ResponseId).ToListAsync());
        }


        public async Task<VOCAnalysis> DeleteDUinSurveyId(string SurveyId)
        {
            var feedback = await GetBySurveyIdAsync(SurveyId);
            if (feedback == null)
            {
                return null;
            }
            feedback.DU = null;
            await _context.SaveChangesAsync();
            return feedback;
        }

        /*  public async Task<VOCAnalysis> GetDUinSurveyId(string SurveyId)
          {
              var feedback = await GetBySurveyIdAsync(SurveyId);
              if (feedback == null)
              {
                  return null;
              }
              *//*feedback.DU = Du;
              await _context.SaveChangesAsync();*//*
              return feedback;
          }*/

        public async Task<List<VOCAnalysis>> GetSurveysByQuarterAsync(string quarter)
        {
            // Parse the quarter string to get the start and end dates
            var dateRange = GetQuarterDateRange(quarter);
            if (dateRange == null)
            {
                return new List<VOCAnalysis>(); // Return empty list if quarter format is invalid
            }

            var (startDate, endDate) = dateRange.Value;

            return await _context.VocAnalyses
                .Where(s => s.Response_Completion_Time >= startDate && s.Response_Completion_Time <= endDate)
                .ToListAsync();
        }


        private (DateTime, DateTime)? GetQuarterDateRange(string quarter)
        {
            var match = Regex.Match(quarter, @"Q(\d) \((\d{4})-(\d{4})\)");
            if (!match.Success) return null; // Invalid format

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


        public async Task SaveVocAnalysesAsync(IEnumerable<VOCAnalysis> vocAnalyses)
        {
            _context.VocAnalyses.AddRange(vocAnalyses);
            await _context.SaveChangesAsync();
        }

        public async Task<List<VOCAnalysis>> GetFeedbackByDUandSurveyId(string Du = null, string SurveyId = null)
        {
            var query = _context.VocAnalyses.AsQueryable();
            if (!string.IsNullOrEmpty(Du))
                query = query.Where(p => p.DU == Du);
            if (!string.IsNullOrEmpty(SurveyId))
                query = query.Where(p => p.SurveyId == SurveyId);
            return await query.Where(v => v != null).ToListAsync();
        }

        public async Task<IEnumerable<VOCAnalysis>> GetFilteredFeedbackAsyncBySurveyId(string surveyId)
        {
            var existingEntries = await _context.VocAnalyses.Where(v => v.SurveyId == surveyId).ToListAsync();
            return existingEntries;

        }

        public async Task<List<VOCAnalysis>> GetBySurveyIdAsyncforSatisfactory_Score(List<string> surveyId)
        {
            return await _context.VocAnalyses
         .Where(v => surveyId.Contains(v.SurveyId)) // Filter using Contains for multiple survey IDs
         .ToListAsync();
        }

        public async Task<VOCAnalysis> GetBySurveyIdAsync(string surveyId)
        {
            return await _context.VocAnalyses.FirstOrDefaultAsync(v => v.SurveyId == surveyId);
        }

        public async Task<List<VOCAnalysis>> GetBySurveyIdsAsync(List<string> surveyIds)
        {
            return await _context.VocAnalyses.Where(v => surveyIds.Contains(v.SurveyId))
        .ToListAsync();
        }

        public async Task SaveVocAnalysesAsync(List<VOCAnalysis> vocAnalyses)
        {
            _context.VocAnalyses.AddRange(vocAnalyses);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateVocAnalysisAsync(VOCAnalysis vocAnalysis)
        {
            var trackedEntity = _context.VocAnalyses.Local.FirstOrDefault(e => e.Id == vocAnalysis.Id);
            if (trackedEntity != null)
            {
                // Detach the existing tracked entity to avoid conflict
                _context.Entry(trackedEntity).State = EntityState.Detached;
            }

            // Attach and update the new entity
            _context.VocAnalyses.Update(vocAnalysis);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateVOCAnalysisAsync(VOCAnalysis vocAnalysis)
        {
            var existingEntry = await _context.VocAnalyses.FirstOrDefaultAsync(v => v.SurveyId == vocAnalysis.SurveyId);
            if (existingEntry == null)
            {
                throw new KeyNotFoundException("Survey ID not found.");
            }

            // Update the properties
            existingEntry.CustomerFocus = vocAnalysis.CustomerFocus;
            existingEntry.PlanningAndControl = vocAnalysis.PlanningAndControl;
            existingEntry.Quality = vocAnalysis.Quality;
            existingEntry.Communication = vocAnalysis.Communication;
            existingEntry.Knowledge = vocAnalysis.Knowledge;
            existingEntry.EngageService = vocAnalysis.EngageService;
            existingEntry.Score = vocAnalysis.Score;
            existingEntry.DU = vocAnalysis.DU;
            existingEntry.Response_Completion_Time = vocAnalysis.Response_Completion_Time;
            await _context.SaveChangesAsync();
        }

        public async Task<List<VOCAnalysis>> GetFeedbackByDU(string Du)
        {
            return await _context.VocAnalyses.Where(v => Du.Contains(v.DU)).ToListAsync();
        }
    }
}
