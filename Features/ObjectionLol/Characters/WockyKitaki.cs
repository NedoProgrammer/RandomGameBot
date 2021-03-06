namespace RandomGameBot.Features.ObjectionLol.Characters;

/*
	Auto generated by my random js script.
	Additional Information:
		Raw Name: Wocky Kitaki
		Formatted Name: WockyKitaki
		Emotion Count: 10
*/
public class WockyKitaki : Character<WockyKitaki.WockyKitakiEmotions>
{
	public enum WockyKitakiEmotions
	{
		Angry = 714,
		Cornered = 713,
		Fight = 712,
		Fight2 = 721,
		Happy = 720,
		Sad = 719,
		Serious = 716,
		Stand = 715,
		Stare = 717,
		Think = 718
	}

	public const string RawName = "Wocky Kitaki";

	public WockyKitaki(string username, WockyKitakiEmotions emotion, string text) : base(username, emotion, text)
	{
	}
}