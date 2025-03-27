using DATEX_ProjectDatabase.Interfaces;
using DATEX_ProjectDatabase.Models;
using DATEX_ProjectDatabase.SignalR;
using Microsoft.AspNetCore.SignalR;

namespace DATEX_ProjectDatabase.Service
{
    public class ProjectJobService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IEmailService _emailService;
        private readonly IHubContext<MailStatusHub> _hubContext;

        public ProjectJobService(
            IProjectRepository projectRepository,
            IEmailService emailService,
            IHubContext<MailStatusHub> hubContext)
        {
            _projectRepository = projectRepository;
            _emailService = emailService;
            _hubContext = hubContext;
        }

        public async Task CheckAndNotifyVocEligibilityAsync()
        {
            var today = DateTime.Today;

            // Get all projects that match the VOC eligibility date
            var projects = (await _projectRepository.GetAllProjectsAsync())
                           .Where(p => p.VOCEligibilityDate == today)
                           .ToList();

            foreach (var project in projects)
            {
                // Assuming PMMails is the property holding the email address of the project manager
                if (!string.IsNullOrEmpty(project.PMMails)) // Check if there's a project manager's email assigned
                {
                    var subject = "VOC Eligibility Notification";
                    var body = $"Dear {project.ProjectManager},\n\nThe VOC eligibility date for project {project.ProjectName} is today.\n\nBest regards,\nYour Team";

                    try
                    {
                        // Send email to the Project Manager's email address
                        bool emailSent = await _emailService.SendEmailAsync(project.PMMails, subject, body);

                        if (emailSent)
                        {
                            // Update the mail status on the project
                            project.MailStatus = "Sent";
                            await _projectRepository.UpdateProjectsAsync(new List<Project> { project });

                            // Notify all clients about the mail status update
                            await _hubContext.Clients.All.SendAsync("MailStatusUpdated");
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log the exception or handle it as needed
                        Console.WriteLine($"Failed to send email to {project.PMMails}: {ex.Message}");
                    }
                }
            }
        }

    }
}
