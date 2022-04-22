using Newtonsoft.Json;

namespace RandomGameBot.Core;

public class Config
{
	public string Token;
	public ulong? RefreshGuildId;

	private static Config? _config;

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

	private Config()
	{
		//Private constructor should only be used for Create() and is used to provide
		//default values for properties in the config.
		Token = "DISCORD_BOT_TOKEN";
	}

	public static void Create() => File.WriteAllText("config.json", JsonConvert.SerializeObject(new Config(), Formatting.Indented));

	public static void Load()
	{
		if (_config != null)
			throw new Exception("Cannot load a config twice!");
		_config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("config.json"));
	}
}