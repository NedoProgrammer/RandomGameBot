using Newtonsoft.Json;

namespace RandomGameBot.Features.ObjectionLol;

/// <summary>
/// A class for containing all objection.lol character preferences
/// in a JSON format.
/// </summary>
public class CharacterPreferences
{
	/// <summary>
	/// Private singleton.
	/// </summary>
	private static CharacterPreferences? _singleton;

	/// <summary>
	/// Singleton.
	/// </summary>
	/// <exception cref="Exception">If singleton was accessed before Load() was called.</exception>
	public static CharacterPreferences Singleton
	{
		get
		{
			if (_singleton == null)
				throw new Exception("Cannot get preferences before Load() is called!");
			return _singleton;
		}
	}
	
	/// <summary>
	/// The preferences.
	/// <remarks>Guild ID --> User ID --> Preference!</remarks>
	/// </summary>
	public Dictionary<ulong, Dictionary<ulong, string>> Preferences = new();

	/// <summary>
	/// Load the preferences from "Data\Preferences.json".
	/// </summary>
	/// <exception cref="InvalidOperationException">If failed to load/parse from the file.</exception>
	public static void Load() => _singleton = new CharacterPreferences
	{
		Preferences = JsonConvert.DeserializeObject<Dictionary<ulong, Dictionary<ulong, string>>>(
			File.ReadAllText("Data\\Preferences.json")) ?? throw new InvalidOperationException()
	};

	/// <summary>
	/// Write the preferences back to "Data\Preferences.json".
	/// </summary>
	public void Save() => File.WriteAllText("Data\\Preferences.json",
		JsonConvert.SerializeObject(Preferences, Formatting.Indented));

	/// <summary>
	/// Add a new preference to the dictionary.
	/// <remarks>This does not check if it is already taken! Use <see cref="Get"/>.</remarks>
	/// </summary>
	/// <param name="guild">Guild ID where the preference change was issued.</param>
	/// <param name="user">User ID who issued the preference changed.</param>
	/// <param name="character">The character selected.</param>
	public void Add(ulong guild, ulong user, string character)
	{
		if (!Preferences.ContainsKey(guild))
			Preferences[guild] = new Dictionary<ulong, string>();
		Preferences[guild][user] = character;
		Save();
	}

	/// <summary>
	/// Get (if present) the user who owns the preference for the character.
	/// </summary>
	/// <param name="guild">Guild ID where the check was issued.</param>
	/// <param name="character">The character selected.</param>
	/// <returns>Null if no one owns the preference, User ID if the owner is present.</returns>
	public ulong? Get(ulong guild, string character)
	{
		if (!Preferences.ContainsKey(guild))
			return null;
		var found = Preferences[guild].FirstOrDefault(x => x.Value == character).Key;
		if (found == 0) return null;
		return found;
	}
}