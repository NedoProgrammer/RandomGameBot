using System.Reflection;
using DSharpPlus;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.SlashCommands;
using Microsoft.Extensions.Logging;
using RandomGameBot.Features.ObjectionLol;
using Serilog;

namespace RandomGameBot.Core;

/// <summary>
/// An enum that is used to describe current state of the bot.
/// </summary>
public enum BotState
{
	Idle,
	Starting,
	Ready,
	Crashed
}

/// <summary>
/// The Discord client.
/// </summary>
public class Bot
{
	/// <summary>
	/// Singleton. :D
	/// </summary>
	public static Bot Singleton = new();
	/// <summary>
	/// Private singleton.
	/// </summary>
	private DiscordClient _client;

	/// <summary>
	/// Constructor that must only be used to create _client.
	/// </summary>
	private Bot()
	{
		//Ignored
	}

	/// <summary>
	/// State of the bot.
	/// </summary>
	public BotState State { get; private set; }
	
	/// <summary>
	/// Start the bot (sync).
	/// </summary>
	public void Start()
	{
		StartAsync().GetAwaiter().GetResult();
	}

	/// <summary>
	/// Start the bot (async).
	/// </summary>
	private async Task StartAsync()
	{
		State = BotState.Starting;
		//I thought I would never see a factory in C#...
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

		//Enable slash commands
		var slashCommands = _client.UseSlashCommands();
		slashCommands.RegisterCommands(Assembly.GetExecutingAssembly());
		slashCommands.SlashCommandInvoked += ConsoleInterface.OnSlashCommandInvoked;
		slashCommands.SlashCommandExecuted += ConsoleInterface.OnSlashCommandExecuted;
		slashCommands.SlashCommandErrored += (sender, args) =>
		{
			Log.Error("{Exception}", args.Exception.ToString());
			return Task.CompletedTask;
		};
		Log.Debug("Slash commands initialized");

		//Enable interactivity
		_client.UseInteractivity();
		Log.Debug("Interactivity initialized");


		//Event handlers that I am too lazy to create custom classes for
		_client.Ready += (_, _) =>
		{
			Log.Information("Client (re)connected");
			State = BotState.Ready;
			return Task.CompletedTask;
		};
		_client.MessageCreated += (sender, args) =>
		{
			/*
			 * Used for Features.ObjectionLol.
			 * A scene is attached to some channel and guild,
			 * so when it does match,
			 * the message gets sent to the scene to handle the message.
			 */
			if (ObjectionLolCommands.Scene == null) return Task.CompletedTask;
			if (ObjectionLolCommands.Scene.ChannelId == args.Channel.Id)
				ObjectionLolCommands.Scene.ProcessMessage(sender, args.Message);

			return Task.CompletedTask;
		};

		await _client.ConnectAsync();
		//Do not return
		await Task.Delay(-1);
	}
}