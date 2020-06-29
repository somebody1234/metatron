using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;

var discord = new DiscordSocketClient();
// discord.MessageReceived += OnMessageReceived;
// discord.ReactionAdded += OnReactionAdded;
// discord.ReactionRemoved += OnReactionRemoved;
// discord.Connected += OnConnected;
await discord.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable("TOKEN"));
await discord.StartAsync();
