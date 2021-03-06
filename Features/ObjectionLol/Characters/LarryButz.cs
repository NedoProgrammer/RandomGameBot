namespace RandomGameBot.Features.ObjectionLol.Characters;

/*
	Auto generated by my random js script.
	Additional Information:
		Raw Name: Larry Butz
		Formatted Name: LarryButz
		Emotion Count: 9
*/
public class LarryButz : Character<LarryButz.LarryButzEmotions>
{
	public enum LarryButzEmotions
	{
		Cornered = 213,
		Cry = 209,
		Happy = 208,
		Nervous = 214,
		Stand = 206,
		Surprised = 212,
		Think = 207,
		Think2 = 210,
		ThumbsUp = 211
	}

	public const string RawName = "Larry Butz";

	public LarryButz(string username, LarryButzEmotions emotion, string text) : base(username, emotion, text)
	{
	}
}