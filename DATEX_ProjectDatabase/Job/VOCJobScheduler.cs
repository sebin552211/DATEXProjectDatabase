using DATEX_ProjectDatabase.Service;

namespace DATEX_ProjectDatabase.Job
{
    public class VOCJobScheduler
    {

        public static async Task CheckVocEligibilityAndSendEmails(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var projectJobService = scope.ServiceProvider.GetRequiredService<ProjectJobService>();
                await projectJobService.CheckAndNotifyVocEligibilityAsync();
            }
        }

    }
}
