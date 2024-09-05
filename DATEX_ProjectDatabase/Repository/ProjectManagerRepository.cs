using DATEX_ProjectDatabase.Data;
using DATEX_ProjectDatabase.Interfaces;
using DATEX_ProjectDatabase.Model;
using Microsoft.EntityFrameworkCore;

namespace DATEX_ProjectDatabase.Repository
{
    public class ProjectManagerRepository : IProjectManagerRepository
    {

        private readonly ApplicationDbContext _context;


        public ProjectManagerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ProjectManagers> GetProjectManagerByProjectIdAsync(int ProjectId)
        {
            return await _context.ProjectManagers
                                 .FirstOrDefaultAsync(pm => pm.ProjectId == ProjectId);
        }

        public async Task AddProjectManagerAsync(ProjectManagers projectManager)
        {
            await _context.ProjectManagers.AddAsync(projectManager);
            await _context.SaveChangesAsync();
        }


        public async Task UpdateProjectManagerAsync(ProjectManagers projectManager)
        {
            _context.ProjectManagers.Update(projectManager);
            await _context.SaveChangesAsync();
        }
    }
}
