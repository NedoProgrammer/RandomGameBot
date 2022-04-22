using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.EventArgs;
using RandomGameBot.Core;
using RandomGameBot.Extensions;
using Serilog;
using Spectre.Console;

namespace RandomGameBot;

public class ConsoleInterface
{
	public static StringWriter RedirectStream = new();
	public static RandomGameBotSink Sink = new(null!);

	private static bool _updateLog = true;
	private static bool _update = true;
	private static bool _updateContext = true;
	private static InteractionContext? _currentContext;

	public static void Start()
	{
		Sink.Emitted += () => _updateLog = true;
		AnsiConsole.Clear();

		var tree = new Tree("Bot");
		var botNode = tree.AddNode("Log");
		var contextNode = tree.AddNode("Context");
		var informationNode = tree.AddNode("Information");
		
		var text = new Markup("[b]Check latest_log.txt for the full log.[/]");

		Task.Run(() => AnsiConsole.Live(tree).AutoClear(false).Start(ctx =>
		{
			while (true)
			{
				if (_updateLog)
				{
					var correctString = string.Join('\n', RedirectStream.ToString().Trim().Split("\n").TakeLast(5)).Replace("[", "[[").Replace("]", "]]");
					botNode.Nodes.Clear();
					botNode.AddNodes(new Panel(correctString), text);
					ctx.Refresh();
					_updateLog = false;	
				}

				if (_update)
				{
					informationNode.Nodes.Clear();
					informationNode.AddNodes(new Markup($"Bot State: {Bot.Singleton.State.ToMarkup()}"));
					ctx.Refresh();
					_update = false;
				}

				if (_updateContext)
				{
					contextNode.Nodes.Clear();
					contextNode.AddNodes(new Panel(_currentContext == null ? "Waiting for commands..": $"Command: /{_currentContext.CommandName}\nUser: {_currentContext.User.Id} ({_currentContext.User.Username})\nGuild: {_currentContext.Guild.Id} ({_currentContext.Guild.Name})\nChannel: {_currentContext.Channel.Id} ({_currentContext.Channel.Name})"));
					ctx.Refresh();
					_updateContext = false;
				}
			}
			// ReSharper disable once FunctionNeverReturns
		}));

		Task.Run(async () =>
		{
			while (true)
			{
				_update = true;
				await Task.Delay(1000);
			}
			// ReSharper disable once FunctionNeverReturns
		});

		Log.Debug("Started TUI update loop");
	}
	
	public static Task OnSlashCommandInvoked(SlashCommandsExtension sender, SlashCommandInvokedEventArgs e)
	{
		Log.Debug("Processing slash command {CommandName} by {UserId} ({Username})", e.Context.CommandName, e.Context.User.Id, e.Context.User.Username);
		_currentContext = e.Context;
		_updateContext = true;
		return Task.CompletedTask;
	}
	
	public static Task OnSlashCommandExecuted(SlashCommandsExtension sender, SlashCommandExecutedEventArgs e)
	{
		_currentContext = null;
		_updateContext = true;
		return Task.CompletedTask;
	}
}