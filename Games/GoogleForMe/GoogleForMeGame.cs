using System.Diagnostics;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.SlashCommands;
using Newtonsoft.Json;
using RandomGameBot.Core;
using RandomGameBot.Extensions;
using Serilog;

namespace RandomGameBot.Games.GoogleForMe;

public class GoogleForMeGame
{
	private DiscordMessage _inviteMessage = null!;

	private readonly CancellationTokenSource _token = new();
	public string ImagePath;
	public string Query;

	public GoogleForMeGame()
	{
		Log.Information("Creating new Google For Me game..");

		Id = Guid.NewGuid().ToString("N");
		Directory.CreateDirectory($"{Environment.CurrentDirectory}\\Games\\GoogleForMe\\{Id}");

		Log.Debug("Successfully created new \"Google For Me\" game with ID {Id}", Id);
	}

	public string Id { get; set; }
	[JsonIgnore] public DiscordChannel Channel { get; private set; } = null!;
	[JsonIgnore] public DiscordGuild Guild { get; private set; } = null!;

	[JsonIgnore] public List<DiscordUser> Players { get; } = new();
	public ulong ChannelId { get; set; }
	public ulong GuildId { get; set; }
	public List<ulong> PlayersIds { get; } = new();

	public async Task PrepareAsync(InteractionContext ctx)
	{
		Channel = await ctx.Guild.CreateChannelAsync($"gfm-{Id}", ChannelType.Text);
		ChannelId = Channel.Id;

		Guild = ctx.Guild;
		GuildId = Guild.Id;

		await Channel.AddOverwriteAsync(Guild.EveryoneRole, Permissions.None, Permissions.SendMessages);

		Save();
		Log.Debug("Successfully prepared a channel for \"Google For Me\" game with ID {Id}", Id);

		Task.Run(() => StartInvite().GetAwaiter().GetResult());
	}

	private async Task StartInvite()
	{
		_inviteMessage = await Channel.SendMessageAsync(new DiscordMessageBuilder()
			.WithContent("тыкните чтобы войти\nначинаем через 5 минут!!")
			.AddComponents(new DiscordButtonComponent(ButtonStyle.Success, "join", "войти")));

		Task.Run(async () =>
		{
			await Task.Delay(10000);
			//await Task.Delay(60000 * 5);
			_token.Cancel();
			await Start();
		});

		await WaitForButton();
	}

	private async Task WaitForButton()
	{
		while (true)
		{
			if (_token.IsCancellationRequested)
				return;

			var result = await _inviteMessage.WaitForButtonAsync("join", _token.Token);
			if (result.TimedOut) continue;
			await result.Result.Interaction.CreateResponseAsync(InteractionResponseType.DeferredMessageUpdate);
			if (PlayersIds.Contains(result.Result.User.Id)) continue;
			Players.Add(result.Result.User);
			PlayersIds.Add(result.Result.User.Id);
			Save();
			var member = await result.Result.Guild.GetMemberAsync(result.Result.User.Id);
			await Channel.AddOverwriteAsync(member, Permissions.SendMessages);
			await Channel.SendMessageAsync($"{member.Mention} вошёл в игру!");
		}
	}

	private void GetImage()
	{
		if (string.IsNullOrEmpty(Query))
			throw new Exception("Query was not set before calling GetImage()!");

		var process = new Process
		{
			StartInfo = new ProcessStartInfo
			{
				FileName = "python",
				Arguments = $"Scripts/GoogleForMe.py \"{Environment.CurrentDirectory}\" {Id} \"{Query}\""
			}
		};
		process.Start();
		process.WaitForExit();
		ImagePath = Path.GetFullPath(Directory.GetFiles($"{Environment.CurrentDirectory}\\Games\\GoogleForMe\\{Id}")
			.First(x => x.Contains("image.")));
		Save();
	}

	private async Task Start()
	{
		await _inviteMessage.DeleteAsync();

		/*if (Players.Count < 3)
		{
			await Channel.SendMessageAsync("ахтунг отмена нас слишком мало!!");
			await Task.Delay(10000);
			await Cleanup();
		}*/

		var mention = Players.Aggregate("просыпайтесь ", (current, player) => current + player.Mention + " ");
		await Channel.SendMessageAsync(mention);

		var status = await Channel.SendMessageAsync("🕔 запускаю ядерный реактор..");
		await Task.Delay(3000);
		await status.ModifyAsync("🔄 выбираю **самый** сложный запрос..");
		Query = RandomExtensions.New().NextElement(Config.Singleton.GoogleForMeQueries);
		await Task.Delay(3000);
		await status.ModifyAsync("🖼️ беру картиночку из гугла..");
		Save();
		GetImage();
	}

	private async Task Cleanup()
	{
		await Channel.DeleteAsync();
		Directory.Delete($"{Environment.CurrentDirectory}\\Games\\GoogleForMe\\{Id}");
	}

	private void Save()
	{
		File.WriteAllText($"{Environment.CurrentDirectory}\\Games\\GoogleForMe\\{Id}\\Data.json",
			JsonConvert.SerializeObject(this, Formatting.Indented));
	}
}