// Services/ChatService.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Zwitscher.Data;
using Zwitscher.Hubs;
using Zwitscher.Models;

namespace Zwitscher.Services
{
    public class ChatService
    {
        private readonly IHubContext<ChatHub> _hub;
        private readonly ApplicationDbContext _db;
        public ChatService(ApplicationDbContext db, IHubContext<ChatHub> hub)
        {
            _db = db;
            _hub = hub;
        }


        public async Task<ChatMessage> SendMessageAsync(string userId, string text, string excludeConnectionId)
        {
            var msg = new ChatMessage
            {
                UserId = userId,
                Text = text,
                CreatedAt = DateTime.UtcNow
            };
            _db.ChatMessages.Add(msg);
            await _db.SaveChangesAsync();
            await _db.Entry(msg).Reference(m => m.User).LoadAsync();

            // Broadcast an alle außer dem Sender
            await _hub.Clients.AllExcept(excludeConnectionId)
                           .SendAsync("ReceiveMessage",
                                      msg.User!.UserName,
                                      msg.Text,
                                      msg.CreatedAt);

            return msg;
        }

        public async Task<List<ChatMessage>> GetLatestMessagesAsync()
        {
            return await _db.ChatMessages
                            .AsNoTracking()
                            .Include(m => m.User)
                            .OrderByDescending(m => m.CreatedAt)
                            .OrderBy(m => m.CreatedAt) // aufsteigend zurückgeben
                            .ToListAsync();
        }
    }
}
