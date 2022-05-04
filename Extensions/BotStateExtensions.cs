using RandomGameBot.Core;

namespace RandomGameBot.Extensions;

/// <summary>
/// Extensions to the <see cref="BotState"/>.
/// </summary>
public static class BotStateExtensions
{
	/// <summary>
	/// Converts the enum name to a beautiful markup string.
	/// </summary>
	/// <param name="state">A BotState value.</param>
	/// <returns>A markup string readable by AnsiConsole.</returns>
	/// <exception cref="ArgumentOutOfRangeException">If invalid state was provided. (how?)</exception>
	public static string ToMarkup(this BotState state)
	{
		return state switch
		{
			BotState.Idle => "[white]Idle[/]",
			BotState.Starting => "[yellow]Starting[/]",
			BotState.Ready => "[green]Ready[/]",
			BotState.Crashed => "[bold red]Crashed![/]",
			_ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
		};
	}
}