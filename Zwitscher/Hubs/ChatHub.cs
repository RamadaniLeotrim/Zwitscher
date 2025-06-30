// Hubs/ChatHub.cs
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Zwitscher.Hubs
{
    public class ChatHub : Hub
    {
        // Wird vom Client aufgerufen, um eine neue Nachricht zu senden
        public async Task SendMessage(string userName, string text, DateTime createdAt)
        {
            // An alle anderen Clients (außer dem Sender) broadcasten
            await Clients.Others.SendAsync("ReceiveMessage", userName, text, createdAt);
        }
    }
}
