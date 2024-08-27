using Microsoft.AspNetCore.SignalR;
using MySqlX.XDevAPI;

namespace DATEX_ProjectDatabase.SignalR
{
    public class MailStatusHub: Hub
    {
        public async Task NotifyMailStatusUpdated()
        {
            await Clients.All.SendAsync("MailStatusUpdated");
        }
    }
}
