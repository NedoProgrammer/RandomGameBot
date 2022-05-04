namespace RandomGameBot.Features.ObjectionLol.Characters;

/*
	Auto generated by my random js script.
	Additional Information:
		Raw Name: Wendy Oldbag
		Formatted Name: WendyOldbag
		Emotion Count: 6
*/
public class WendyOldbag : Character<WendyOldbag.WendyOldbagEmotions>
{
	public enum WendyOldbagEmotions
	{
		Angry = 231,
		Confident = 228,
		Damage = 233,
		InLove = 232,
		Stand = 230,
		Wave = 229
	}

	public const string RawName = "Wendy Oldbag";

	public WendyOldbag(string username, WendyOldbagEmotions emotion, string text) : base(username, emotion, text)
	{
	}
}