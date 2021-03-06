namespace RandomGameBot.Features.ObjectionLol.Characters;

/*
	Auto generated by my random js script.
	Additional Information:
		Raw Name: Angel Starr
		Formatted Name: AngelStarr
		Emotion Count: 7
*/
public class AngelStarr : Character<AngelStarr.AngelStarrEmotions>
{
	public enum AngelStarrEmotions
	{
		Angry = 419,
		Confident = 422,
		Cornered = 420,
		Damage = 425,
		Give = 423,
		Happy = 421,
		Stand = 424
	}

	public const string RawName = "Angel Starr";

	public AngelStarr(string username, AngelStarrEmotions emotion, string text) : base(username, emotion, text)
	{
	}
}