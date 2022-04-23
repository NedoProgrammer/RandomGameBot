using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.SlashCommands;
using Newtonsoft.Json;
using RandomGameBot.Core;
using Serilog;

namespace RandomGameBot.Games.GoogleForMe;

public class GoogleForMeGame
{
	public string Id { get; set; }
	[JsonIgnore] public DiscordChannel Channel { get; private set; }
	[JsonIgnore] public DiscordGuild Guild { get; private set; }

	[JsonIgnore] public List<DiscordUser> Players { get; } = new();
	public ulong ChannelId { get; set; }
	public ulong GuildId { get; set; }
	public List<ulong> PlayersIds { get; } = new();

	private DiscordMessage _inviteMessage;
	
	public GoogleForMeGame()
	{
		Log.Information("Creating new Google For Me game..");
		
		Id = Guid.NewGuid().ToString("N");
		Directory.CreateDirectory($"{Environment.CurrentDirectory}\\Games\\GoogleForMe\\{Id}");

		Log.Debug("Successfully created new \"Google For Me\" game with ID {Id}", Id);
	}

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

	private CancellationTokenSource _token = new();
	private async Task StartInvite()
	{
		_inviteMessage = await Channel.SendMessageAsync(new DiscordMessageBuilder()
			.WithContent("тыкните чтобы войти\nначинаем через 5 минут!!")
			.AddComponents(new DiscordButtonComponent(ButtonStyle.Success, "join", "войти")));

		Task.Run(async () =>
		{
			await Task.Delay(60000 * 5);
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
			var member = await result.Result.Guild.GetMemberAsync(result.Result.User.Id);
			await Channel.AddOverwriteAsync(member, Permissions.SendMessages);
			await Channel.SendMessageAsync($"{member.Mention} вошёл в игру!");
		}
	}

	private async Task Start()
	{
		
	}

	private void Save() => File.WriteAllText($"{Environment.CurrentDirectory}\\Games\\GoogleForMe\\{Id}\\Data.json", JsonConvert.SerializeObject(this, Formatting.Indented));
}