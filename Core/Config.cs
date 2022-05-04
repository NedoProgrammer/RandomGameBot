using Newtonsoft.Json;

namespace RandomGameBot.Core;

/// <summary>
/// Config for the client.
/// </summary>
public class Config
{
	/// <summary>
	/// Private singleton.
	/// </summary>
	private static Config? _config;
	/// <summary>
	/// Used for Games.GoogleForMe.
	/// Possible queries for the GoogleForMe game.
	/// </summary>
	public string[] GoogleForMeQueries;
	/// <summary>
	/// If specified, refreshes slash commands instantly for the guild with such ID.
	/// </summary>
	public ulong? RefreshGuildId;
	/// <summary>
	/// Token of the bot.
	/// </summary>
	public string Token;

	
	/// <summary>
	/// Private constructor should only be used for Create() and is used to provide
	/// default values for properties in the config.
	/// </summary>
	private Config()
	{
		Token = "DISCORD_BOT_TOKEN";
	}

	/// <summary>
	/// Singleton. :D
	/// </summary>
	/// <exception cref="Exception">If the config was accessed before it was loaded using Config.Load()</exception>
	public static Config Singleton
	{
		get
		{
			if (_config == null)
				throw new Exception("Cannot get config before it is loaded!");
			return _config;
		}
		private set => _config = value;
	}

	/// <summary>
	/// Create a sample config and save it to "config.json".
	/// </summary>
	public static void Create()
	{
		File.WriteAllText("config.json", JsonConvert.SerializeObject(new Config(), Formatting.Indented));
	}

	/// <summary>
	/// Load the config from "config.json".
	/// </summary>
	/// <exception cref="Exception">If Load() was called twice, or the function failed to parse the config.</exception>
	public static void Load()
	{
		if (_config != null)
			throw new Exception("Cannot load a config twice!");
		_config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("config.json"));
		if (_config == null)
			throw new Exception("Failed to load config!");
		_config.GoogleForMeQueries = File.ReadAllLines("Data/GoogleForMe.txt");
	}
}