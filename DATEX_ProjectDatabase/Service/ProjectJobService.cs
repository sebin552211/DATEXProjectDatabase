using DATEX_ProjectDatabase.Interfaces;

namespace DATEX_ProjectDatabase.Service
{
    public class ProjectJobService
    {

        private readonly IProjectRepository _projectRepository;
        private readonly IProjectManagerRepository _projectManagerRepository;
        private readonly IEmailService _emailService;

        public ProjectJobService(IProjectRepository projectRepository, IProjectManagerRepository projectManagerRepository, IEmailService emailService)
        {
            _projectRepository = projectRepository;
            _projectManagerRepository = projectManagerRepository;
            _emailService = emailService;
        }

        public async Task CheckAndNotifyVocEligibilityAsync()
        {
            var today = DateTime.Today;
            var projects = (await _projectRepository.GetAllProjectsAsync()).Where(p => p.VOCEligibilityDate == today).ToList();

            foreach (var project in projects)
            {
                var manager = await _projectManagerRepository.GetProjectManagerByProjectIdAsync(project.ProjectId);

                if (manager != null)
                {
                    var subject = "VOC Eligibility Notification";
                    var body = $"Dear {manager.Name},\n\nThe VOC eligibility date for project {project.ProjectName} is today.\n\nBest regards,\nYour Team";

                    try
                    {
                        await _emailService.SendEmailAsync(manager.Email, subject, body);
                    }
                    catch (Exception ex)
                    {
                        // Log the exception or handle it as needed
                        Console.WriteLine($"Failed to send email to {manager.Email}: {ex.Message}");
                    }
                }
            }
        }

    }
}