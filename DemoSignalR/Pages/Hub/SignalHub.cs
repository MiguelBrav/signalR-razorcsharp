namespace PruebaSignalR.Pages.Hub
{
    using Microsoft.AspNetCore.SignalR;
    using System.Collections.Concurrent;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    public class SignalHub : Hub
    {
        private static readonly ConcurrentDictionary<string, (string userId, string userEmail)> ConnectedClients = new ConcurrentDictionary<string, (string userId, string userEmail)>();

        public override Task OnConnectedAsync()
        {
            var userId = Context.GetHttpContext().Request.Query["userId"];
            var userEmail = Context.GetHttpContext().Request.Query["userEmail"];

            var existingConnection = ConnectedClients.FirstOrDefault(x => x.Value.userId == userId && x.Value.userEmail == userEmail);

            if (existingConnection.Equals(default(KeyValuePair<string, (string, string)>)))
            {
                var connectionId = Context.ConnectionId;

                ConnectedClients.TryAdd(connectionId, (userId, userEmail));
            }

            Clients.All.SendAsync("UpdateConnectedUsers", GetConnectedClientsJson());

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            ConnectedClients.TryRemove(Context.ConnectionId, out _);

            Clients.All.SendAsync("UpdateConnectedUsers", GetConnectedClientsJson());

            return base.OnDisconnectedAsync(exception);
        }

        private string GetConnectedClientsJson()
        {
            var clientsJson = ConnectedClients.Select(kvp => new
            {
                ConnectionId = kvp.Key,
                UserId = kvp.Value.userId,
                UserEmail = kvp.Value.userEmail
            }).ToList();

            return JsonSerializer.Serialize(clientsJson);
        }
        public Task SendMessage(string message)
        {

            if (ConnectedClients.TryGetValue(Context.ConnectionId, out var clientInfo))
            {
                var userId = clientInfo.userId;
                var userEmail = clientInfo.userEmail;

                var chatMessage = new
                {
                    UserId = userId,
                    UserEmail = userEmail,
                    Message = message,
                    Timestamp = DateTime.UtcNow
                };

                return Clients.All.SendAsync("ReceiveMessage", chatMessage);
            }
            else
            {
                return Task.CompletedTask;
            }
        }
    }
}
