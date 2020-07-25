using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace Metatron {
    internal static class Discord {
        internal static DiscordSocketClient Client;
    
        internal async static Task Initialize() {
            Client = new DiscordSocketClient();
            // Client.MessageReceived += OnMessageReceived;
            // Client.ReactionAdded += OnReactionAdded;
            // Client.ReactionRemoved += OnReactionRemoved;
            // Client.Connected += OnConnected;
            await Client.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable("TOKEN"));
            await Client.StartAsync();
        }
    }
}
