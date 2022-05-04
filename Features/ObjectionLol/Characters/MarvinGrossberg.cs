namespace RandomGameBot.Features.ObjectionLol.Characters;

/*
	Auto generated by my random js script.
	Additional Information:
		Raw Name: Marvin Grossberg
		Formatted Name: MarvinGrossberg
		Emotion Count: 2
*/
public class MarvinGrossberg : Character<MarvinGrossberg.MarvinGrossbergEmotions>
{
	public enum MarvinGrossbergEmotions
	{
		Cornered = 359,
		Stand = 360
	}

	public const string RawName = "Marvin Grossberg";

	public MarvinGrossberg(string username, MarvinGrossbergEmotions emotion, string text) : base(username, emotion,
		text)
	{
	}
}