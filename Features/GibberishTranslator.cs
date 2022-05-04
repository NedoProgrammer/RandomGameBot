using DSharpPlus;
using DSharpPlus.SlashCommands;

namespace RandomGameBot.Features;

public class GibberishTranslator : ApplicationCommandModule
{
	private readonly string _english = "`qwertyuiop[]asdfghjkl;'zxcvbnm,.`QWERTYUIOP[]ASDFGHJKL;'ZXCVBNM,.";
	private readonly string _russian = "ёйцукенгшщзхъфывапролджэячсмитьбюЁЙЦУКЕНГШЩЗХЪФЫВАПРОЛДЖЭЯЧСМИТЬБЮ";

	[ContextMenu(ApplicationCommandType.MessageContextMenu, "латиница -> кириллица")]
	public async Task LatinToCyrillic(ContextMenuContext ctx)
	{
		var converted = new string(ctx.TargetMessage.Content.Select(x =>
		{
			var i = _english.IndexOf(x);
			return i != -1 ? _russian[i] : x;
		}).ToArray());
		await ctx.CreateResponseAsync(
			$"когда {ctx.TargetMessage.Author.Username} сказал(а):\n> {ctx.TargetMessage.Content}\nя думаю, он(а) имел(а) ввиду:\n> {converted}");
	}

	[ContextMenu(ApplicationCommandType.MessageContextMenu, "кириллица -> латиница")]
	public async Task CyrillicToLatin(ContextMenuContext ctx)
	{
		var converted = new string(ctx.TargetMessage.Content.Select(x =>
		{
			var i = _russian.IndexOf(x);
			return i != -1 ? _english[i] : x;
		}).ToArray());
		await ctx.CreateResponseAsync(
			$"когда {ctx.TargetMessage.Author.Username} сказал(а):\n> {ctx.TargetMessage.Content}\nя думаю, он(а) имел(а) ввиду:\n> {converted}");
	}
}