using System.Reflection;
using DSharpPlus;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.EventArgs;
using Microsoft.Extensions.Logging;
using Serilog;

namespace RandomGameBot.Core;

public enum BotState
{
	Idle,
	Starting,
	Ready,
	Crashed
}

public class Bot
{
	private DiscordClient _client;

	public static Bot Singleton = new();

	public BotState State { get; private set; }
	
	private Bot()
	{
		//Ignored
	}
	
	
	public void Start()
	{
		StartAsync().GetAwaiter().GetResult();
	}

	private async Task StartAsync()
	{
		State = BotState.Starting;
		var factory = new LoggerFactory().AddSerilog();
		_client = new DiscordClient(new DiscordConfiguration
		{
			Intents = DiscordIntents.All,
			Token = Config.Singleton.Token,
			TokenType = TokenType.Bot,
			LoggerFactory = factory,
			#if DEBUG
			MinimumLogLevel = LogLevel.Debug
			#else
			MinimumLogLevel = LogLevel.Information
			#endif
		});
		Log.Information("Client created");

		var slashCommands = _client.UseSlashCommands();
		slashCommands.RegisterCommands(Assembly.GetExecutingAssembly(), Config.Singleton.RefreshGuildId);
		slashCommands.SlashCommandInvoked += ConsoleInterface.OnSlashCommandInvoked;
		slashCommands.SlashCommandExecuted += ConsoleInterface.OnSlashCommandExecuted;
		Log.Debug("Slash commands initialized");

		_client.Ready += (_, _) =>
		{
			Log.Information("Client (re)connected");
			State = BotState.Ready;
			return Task.CompletedTask;
		};

		await _client.ConnectAsync();
		await Task.Delay(-1);
	}

	
}