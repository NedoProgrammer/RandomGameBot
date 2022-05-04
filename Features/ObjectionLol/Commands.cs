using System.Diagnostics;
using System.Reflection;
using System.Text;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Enums;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.SlashCommands;
using MoreLinq;
using RandomGameBot.Core;
using RandomGameBot.Features.ObjectionLol.Characters;
using Serilog;

namespace RandomGameBot.Features.ObjectionLol;

[SlashCommandGroup("objection", "команды для управления функцией \"чат -> objection.lol\"")]
public class ObjectionLolCommands : ApplicationCommandModule
{
	public static List<Type> Characters = null!;

	public static Scene? Scene;

	public static void Initialize()
	{
		Log.Information("Loading objection.lol characters (reflection)");
		Characters = Assembly.GetExecutingAssembly().GetTypes().Where(x =>
				x.BaseType is {IsGenericType: true} && x.BaseType?.GetGenericTypeDefinition() == typeof(Character<>))
			.ToList();
		Log.Debug("Loaded {CharacterCount} characters", Characters.Count);
	}

	[SlashCommand("settings", "настроить своего персонажа в objection.lol")]
	public async Task Settings(InteractionContext ctx)
	{
		await ctx.DeferAsync();
		var viewSheet = await ViewCharacterSheet(ctx);
		if (viewSheet == null) return;
		if (viewSheet == true)
		{
			await ctx.EditResponseAsync(new DiscordWebhookBuilder()
				.AddFile("cheatsheet.png", new FileStream("Data\\cheatsheet.png", FileMode.Open, FileAccess.Read)));
			return;
		}

		var character = await SelectCharacter(ctx);
		if (character == null) return;
		await ctx.EditResponseAsync(new DiscordWebhookBuilder()
			.WithContent("секунду.."));
		var characterOwner = CharacterPreferences.Singleton.Get(ctx.Guild.Id, character);
		if (characterOwner != null && characterOwner != ctx.User.Id)
			await ctx.EditResponseAsync(new DiscordWebhookBuilder()
				.WithContent(
					$"этот персонаж уже занят {(await ctx.Guild.GetMemberAsync((ulong) characterOwner)).Username}!"));
		else if (characterOwner != null && characterOwner == ctx.User.Id)
			await ctx.EditResponseAsync(new DiscordWebhookBuilder()
				.WithContent("так он у тебя уже стоит!"));
		else
		{
			CharacterPreferences.Singleton.Add(ctx.Guild.Id, ctx.User.Id, character);
			await ctx.EditResponseAsync(new DiscordWebhookBuilder()
				.WithContent(DiscordEmoji.FromName(ctx.Client, ":+1:", false)));
		}
	}

	private async Task<bool?> ViewCharacterSheet(InteractionContext ctx)
	{
		var message = await ctx.EditResponseAsync(new DiscordWebhookBuilder()
			.AddComponents(new DiscordButtonComponent(ButtonStyle.Success, "yes", "да"),
				new DiscordButtonComponent(ButtonStyle.Danger, "no", "нет"))
			.WithContent("хочешь ли посмотреть доступных персонажей перед выбором?"));
		var response = await message.WaitForButtonAsync(ctx.User, TimeSpan.FromMinutes(1));
		await response.Result.Interaction.CreateResponseAsync(InteractionResponseType.DeferredMessageUpdate);
		if (response.TimedOut)
		{
			await ctx.EditResponseAsync(new DiscordWebhookBuilder()
				.WithContent("ну как так то а за минуту не выбрать.. я обиделся"));
			return null;
		}

		return response.Result.Id == "yes";
	}


	private CustomPaginationRequest _request;
	private async Task<string?> SelectCharacter(InteractionContext ctx)
	{
		var names = Characters
			.Select(x => (new string(((string) x.GetField("RawName")!.GetValue(null)!).Take(32).ToArray()), x.Name))
			.Batch(25)
			.ToList();
		var dropdowns = names.Select((collection, index) => new DiscordSelectComponent($"select{index}", "выберите..",
			collection.Select(x => new DiscordSelectComponentOption(x.Item1, x.Name)))).ToList();
		var strings = names
			.Select(collection =>
				collection.Aggregate("на этой странице:\n", (current, character) => current + $"• {character.Item1}\n"))
			.Select(str => str.Remove(str.LastIndexOf("\n", StringComparison.Ordinal))).ToList();
		var interactivity = ctx.Client.GetInteractivity();
		var pages = strings.Select(x => new Page("", new DiscordEmbedBuilder()
			.WithTitle("выбор персонажа")
			.WithColor(DiscordColor.Gold)
			.WithDescription(x)));
		await ctx.EditResponseAsync(new DiscordWebhookBuilder()
			.AddEmbed(pages.First().Embed));
		_request = new CustomPaginationRequest(await ctx.GetOriginalResponseAsync(), ctx.User,
			PaginationBehaviour.WrapAround, PaginationDeletion.DeleteEmojis, new PaginationEmojis(),
			TimeSpan.FromMinutes(2), pages.ToArray());
		await interactivity.WaitForCustomPaginationAsync(_request);
		var message = await ctx.EditResponseAsync(new DiscordWebhookBuilder()
			.WithContent("выбирай!")
			.AddComponents(dropdowns[_request.Result]));
		var response = await message.WaitForSelectAsync(ctx.User, $"select{_request.Result}", TimeSpan.FromMinutes(1));
		await response.Result.Interaction.CreateResponseAsync(InteractionResponseType.DeferredMessageUpdate);
		if (response.TimedOut)
		{
			await ctx.EditResponseAsync(new DiscordWebhookBuilder()
				.WithContent("ну ёмаё ну как так то.."));
			return null;
		}

		return response.Result.Values[0];
	}

	[SlashCommand("record", "начать запись чата")]
	public async Task StartRecord(InteractionContext ctx)
	{
		if (Scene != null)
		{
			await ctx.CreateResponseAsync("запись уже начата!! куда ещё..", false);
		}
		else
		{
			await ctx.CreateResponseAsync(DiscordEmoji.FromName(ctx.Client, ":+1:", false), false);
			Scene = new Scene
			{
				Creator = ctx.User.Id,
				ChannelId = ctx.Channel.Id,
				GuildId = ctx.Guild.Id
			};
			Scene.AddPreferences();
		}
	}

	[SlashCommand("stop", "прекратить запись чата")]
	public async Task StopRecord(InteractionContext ctx)
	{
		if (Scene == null)
			await ctx.CreateResponseAsync("а чё заканчивать..");
		else
		{
			if (Scene.Creator != ctx.User.Id)
			{
				await ctx.CreateResponseAsync("а вот нельзя тебе!! не ты начинал!!");
				return;
			}
			var (json, additionalInfo) = Scene.ToJson();
			Task.Run(() => CreateVideo(ctx.Channel, json, additionalInfo));
			Scene = null;
			await ctx.CreateResponseAsync(DiscordEmoji.FromName(ctx.Client, ":+1:", false), false);
		}
	}

	private async Task CreateVideo(DiscordChannel channel, string json, string additional)
	{
		await File.WriteAllTextAsync($"{Environment.CurrentDirectory}\\Scripts\\ObjectionLolRenderer\\scene.objection", Convert.ToBase64String(Encoding.UTF8.GetBytes(json)));
		await File.WriteAllTextAsync($"{Environment.CurrentDirectory}\\Scripts\\ObjectionLolRenderer\\additional.json", additional);
		var process = new Process
		{
			StartInfo = new ProcessStartInfo
			{
				FileName = "node",
				WorkingDirectory = $"{Environment.CurrentDirectory}\\Scripts\\ObjectionLolRenderer",
				Arguments = "index.js"
			}
		};
		process.Start();
		await process.WaitForExitAsync();

		var video = $"{Environment.CurrentDirectory}\\Scripts\\ObjectionLolRenderer\\objection.webm";
		await channel.SendMessageAsync(new DiscordMessageBuilder()
			.WithFile("objection.webm", new FileStream(video, FileMode.Open, FileAccess.Read)));
		GC.WaitForPendingFinalizers();
		await Task.Delay(5000);
		File.Delete(video);
	}
}