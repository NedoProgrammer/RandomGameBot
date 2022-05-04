using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace RandomGameBot.Games.GoogleForMe;

[SlashCommandGroup("googleforme", "команды для игры в \"загугли-ка\"")]
public class GoogleForMeCommands : ApplicationCommandModule
{
	[SlashCommand("create", "создать новую игру в \"загугли-ка\"")]
	public async Task ExecuteCreate(InteractionContext ctx)
	{
		await ctx.DeferAsync();

		var game = new GoogleForMeGame();
		await game.PrepareAsync(ctx);

		await ctx.EditResponseAsync(new DiscordWebhookBuilder()
			.WithContent($"во {game.Channel.Mention}"));
	}
}