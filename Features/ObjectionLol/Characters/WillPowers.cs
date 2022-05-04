namespace RandomGameBot.Features.ObjectionLol.Characters;

/*
	Auto generated by my random js script.
	Additional Information:
		Raw Name: Will Powers
		Formatted Name: WillPowers
		Emotion Count: 5
*/
public class WillPowers : Character<WillPowers.WillPowersEmotions>
{
	public enum WillPowersEmotions
	{
		Dull = 585,
		Happy = 584,
		Sad = 587,
		Stand = 588,
		Think = 586
	}

	public const string RawName = "Will Powers";

	public WillPowers(string username, WillPowersEmotions emotion, string text) : base(username, emotion, text)
	{
	}
}