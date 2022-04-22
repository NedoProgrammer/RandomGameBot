using RandomGameBot.Core;

namespace RandomGameBot.Extensions;

public static class BotStateExtensions
{
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