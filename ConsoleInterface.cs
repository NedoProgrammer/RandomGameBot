using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.EventArgs;
using RandomGameBot.Core;
using RandomGameBot.Extensions;
using Serilog;
using Spectre.Console;

namespace RandomGameBot;

/// <summary>
/// The TUI interface of the bot.
/// </summary>
public class ConsoleInterface
{
	/// <summary>
	/// The stream where Console output is redirected.
	/// </summary>
	public static StringWriter RedirectStream = new();
	/// <summary>
	/// The Serilog sink which outputs messages to the console.
	/// </summary>
	public static RandomGameBotSink Sink = new(null!);

	/// <summary>
	/// Should the TUI update the log panel?
	/// </summary>
	private static bool _updateLog = true;
	/// <summary>
	/// Should the TUI update?
	/// </summary>
	private static bool _update = true;
	/// <summary>
	/// Should the TUI update the context panel?
	/// </summary>
	private static bool _updateContext = true;
	/// <summary>
	/// Current Discord interaction context. (command context)
	/// </summary>
	private static InteractionContext? _currentContext;
	
	/// <summary>
	/// Start the TUI update tasks.
	/// </summary>
	public static void Start()
	{
		Sink.Emitted += () => _updateLog = true;
		AnsiConsole.Clear();

		//UI elements
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
					//Correct Serilog messages to use [[]] instead of [] (Spectre.Console uses [] for markup)
					var correctString = string.Join('\n', RedirectStream.ToString().Trim().Split("\n").TakeLast(5))
						.Replace("[", "[[").Replace("]", "]]");
					//Refresh nodes
					botNode.Nodes.Clear();
					botNode.AddNodes(new Panel(correctString), text);
					ctx.Refresh();
					_updateLog = false;
				}

				if (_update)
				{
					//Refresh nodes
					informationNode.Nodes.Clear();
					informationNode.AddNodes(new Markup($"Bot State: {Bot.Singleton.State.ToMarkup()}"));
					ctx.Refresh();
					_update = false;
				}

				if (_updateContext)
				{
					//Refresh nodes
					contextNode.Nodes.Clear();
					contextNode.AddNodes(new Panel(_currentContext == null
						? "Waiting for commands.."
						: $"Command: /{_currentContext.CommandName}\nUser: {_currentContext.User.Id} ({_currentContext.User.Username})\nGuild: {_currentContext.Guild.Id} ({_currentContext.Guild.Name})\nChannel: {_currentContext.Channel.Id} ({_currentContext.Channel.Name})"));
					ctx.Refresh();
					_updateContext = false;
				}
			}
			// ReSharper disable once FunctionNeverReturns
		}));

		//Start the task to update the information label each second.
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

	#region Discord Event Handlers
	/// <summary>
	/// Update current context.
	/// </summary>
	/// <param name="sender">Not used.</param>
	/// <param name="e">Information about the command.</param>
	/// <returns>Task.CompletedTask</returns>
	public static Task OnSlashCommandInvoked(SlashCommandsExtension sender, SlashCommandInvokedEventArgs e)
	{
		Log.Debug("Processing slash command {CommandName} by {UserId} ({Username})", e.Context.CommandName,
			e.Context.User.Id, e.Context.User.Username);
		_currentContext = e.Context;
		_updateContext = true;
		return Task.CompletedTask;
	}

	/// <summary>
	/// Update (reset) current context.
	/// </summary>
	/// <param name="sender">Not used.</param>
	/// <param name="e">Information about the command.</param>
	/// <returns>Task.CompletedTask</returns>
	public static Task OnSlashCommandExecuted(SlashCommandsExtension sender, SlashCommandExecutedEventArgs e)
	{
		_currentContext = null;
		_updateContext = true;
		return Task.CompletedTask;
	}
	#endregion
}