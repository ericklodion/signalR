using ErickEspinosa.SignalR.WebApi.Models;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;

namespace ErickEspinosa.SignalR.WebApi.Hubs
{
    public class ChatHub : Hub
    {
        private readonly List<Message> _messages;
        public ChatHub() => _messages = new List<Message>();

        public void NewMessage(string userName, string message)
        {
            Clients.All.SendAsync("newMessage", userName, message);
            _messages.Add(new Message
            {
                Text = message,
                UserName = userName
            });
        }

        public void NewUser(string username, string connectionId)
        {
            Clients.Client(connectionId).SendAsync("previousMessages", _messages);
            Clients.All.SendAsync("newUser", username);
        }
    }
}