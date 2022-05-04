using System.Text.RegularExpressions;
using DSharpPlus;
using DSharpPlus.Entities;
using Newtonsoft.Json;
using RandomGameBot.Extensions;
using RandomGameBot.Features.ObjectionLol.Helper;
using RandomGameBot.Features.ObjectionLol.Wrapper;
using Serilog;
using Group = RandomGameBot.Features.ObjectionLol.Wrapper.Group;

namespace RandomGameBot.Features.ObjectionLol;

public class Scene
{
	public ulong ChannelId;
	public ulong GuildId;
	public ulong Creator;
	public List<object> Frames = new();
	public Dictionary<ulong, int> UserMap = new();
	public Wrapper.Scene Wrapped = new();
	public AdditionalInformation Information = new();

	public void AddPreferences()
	{
		if (!CharacterPreferences.Singleton.Preferences.ContainsKey(GuildId)) return;
		var preferences = CharacterPreferences.Singleton.Preferences[GuildId];
		foreach (var p in preferences)
			UserMap[p.Key] = ObjectionLolCommands.Characters.FindIndex(x => x.Name == p.Value);
	}

	private string[] _extensions = {".gif", ".jpg", ".png"};
	public void ProcessMessage(DiscordClient client, DiscordMessage message)
	{
		if (!UserMap.ContainsKey(message.Author.Id))
		{
			int index;
			do
			{
				index = RandomExtensions.New().Next(0, ObjectionLolCommands.Characters.Count);
			} while (UserMap.ContainsValue(index));

			UserMap[message.Author.Id] = index;
			Log.Debug("{User} is now {CharacterName}", message.Author.Username,
				ObjectionLolCommands.Characters[index].Name);
		}

		var character = ObjectionLolCommands.Characters[UserMap[message.Author.Id]];
		var emotions = character.GetNestedTypes()[0];
		var firstEmotion = Enum.Parse(emotions, RandomExtensions.New().NextElement(emotions.GetEnumNames()));
		var content = message.Content;
		var urlPattern = "(?:http(s)?:\\/\\/)?[\\w.-]+(?:\\.[\\w\\.-]+)+[\\w\\-\\._~:\\/?#[\\]@!\\$&'\\(\\)\\*\\+,;=.]+$";
		var urlMatch = Regex.Matches(content, urlPattern).FirstOrDefault(x => _extensions.Contains(string.Join("", x.Value.TakeLast(4).ToArray())));
		var emojiPattern = "(<a?)?:\\w+:(\\d{18}>)?";
		var emojiMatch = Regex.Match(content, emojiPattern);
		var pingPattern = "<(?:@[!&]?|#)\\d+>";
		foreach (var match in Regex.Matches(content, pingPattern).ToList())
		{
			var id = ulong.Parse(Regex.Match(match.Value, @"\d+").Value);
			//role
			if (match.Value.StartsWith("<@&"))
			{
				var role = client.Guilds.First(x => x.Value.Roles.Any(y => y.Key == id)).Value.GetRole(id).Name;
				content = content.Replace(match.Value, "@" + role);
			}
			//user
			else if (match.Value.StartsWith("<@!") || match.Value.StartsWith("<@"))
			{
				var user = client.GetUserAsync(id).GetAwaiter().GetResult().Username;
				content = content.Replace(match.Value, "@" + user);
			}
			//channel
			else if (match.Value.StartsWith("<#"))
			{
				var channel = client.GetChannelAsync(id).GetAwaiter().GetResult().Name;
				content = content.Replace(match.Value, "#" + channel);
			}
		}

		content = Regex.Replace(content, urlPattern, "");
		content = Regex.Replace(content, emojiPattern, "");

		if (message.Attachments.Count > 0)
		{
			Information.evidence.Add(new EvidenceTag
			{
				url = message.Attachments[0].Url,
				index = Frames.Count
			});
			content = content.Replace(message.Attachments[0].Url, "");
		}
		else if (urlMatch != null)
		{
			Information.evidence.Add(new EvidenceTag
			{
				url = urlMatch.Value,
				index = Frames.Count
			});
		}
		else if (emojiMatch.Success)
		{
			var id = Regex.Match(emojiMatch.Value, "\\d+");
			var emoji = id.Success ? DiscordEmoji.FromGuildEmote(client, ulong.Parse(id.Value)) : DiscordEmoji.FromName(client, emojiMatch.Value.Replace("<", "").Replace(">", ""));
			Information.evidence.Add(new EvidenceTag
			{
				url = emoji.Url,
				index = Frames.Count
			});
		}
		Frames.Add(Activator.CreateInstance(character, message.Author.Username, firstEmotion, "[#evdh]" + content)!);
	}

	public (string, string) ToJson()
	{
		Wrapped.groups = new List<Group>
		{
			new()
			{
				name = "Main",
				iid = 1,
				frames = new List<Frame>()
			}
		};
		var count = 1;
		foreach (var frame in Frames)
			try
			{
				Wrapped.groups[0].frames
					.Add((Frame) frame.GetType().GetMethod("ToFrame").Invoke(frame, new[] {(object) count}));
				count++;
			}
			catch (Exception e)
			{
				Log.Error("{Exception}", e.ToString());
			}

		return (JsonConvert.SerializeObject(Wrapped), JsonConvert.SerializeObject(Information, Formatting.Indented));
	}
}