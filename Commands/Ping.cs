using DSharpPlus.SlashCommands;
using RandomGameBot.Extensions;

namespace RandomGameBot.Commands;

/// <summary>
/// A command to see if the bot is alive.
/// </summary>
public class Ping : ApplicationCommandModule
{
	/// <summary>
	/// Possible responses to the request.
	/// </summary>
	private static readonly string[] Responses =
	{
		"а я был жив?", "1000 - 7", "ага", "ой не", "An unexpected error occured.",
		"<@492327234992865290> просыпайся зовут"
	};

	/// <summary>
	/// Execute the command.
	/// </summary>
	/// <param name="ctx">Interaction context.</param>
	[SlashCommand("ping", "посмотреть, жив ли бот")]
	public async Task Execute(InteractionContext ctx)
	{
		await ctx.CreateResponseAsync(RandomExtensions.New().NextElement(Responses), true);
	}
}