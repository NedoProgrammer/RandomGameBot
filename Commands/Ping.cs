using DSharpPlus.SlashCommands;
using RandomGameBot.Extensions;

namespace RandomGameBot.Commands;

public class Ping: ApplicationCommandModule
{
	private static readonly string[] Responses = {"а я был жив?", "1000 - 7", "ага", "ой не", "An unexpected error occured.", "<@492327234992865290> просыпайся зовут"};
	
	[SlashCommand("ping", "посмотреть, жив ли бот")]
	public async Task Execute(InteractionContext ctx) => await ctx.CreateResponseAsync(RandomExtensions.New().NextElement(Responses), true);
}