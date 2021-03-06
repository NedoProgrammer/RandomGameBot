namespace RandomGameBot.Features.ObjectionLol.Characters;

/*
	Auto generated by my random js script.
	Additional Information:
		Raw Name: Apollo Justice
		Formatted Name: ApolloJustice
		Emotion Count: 14
*/
public class ApolloJustice : Character<ApolloJustice.ApolloJusticeEmotions>
{
	public enum ApolloJusticeEmotions
	{
		Confident = 416,
		Cornered = 415,
		Damage = 63,
		DamageCustom = 60,
		DeskSlam = 402,
		Point = 56,
		Read = 403,
		Silly = 58,
		SillyEyesClosed = 61,
		Smile = 57,
		Stand = 405,
		Think = 404,
		ThinkEyesClosed = 62,
		Yell = 55
	}

	public const string RawName = "Apollo Justice";

	public ApolloJustice(string username, ApolloJusticeEmotions emotion, string text) : base(username, emotion, text)
	{
	}
}