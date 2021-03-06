namespace RandomGameBot.Features.ObjectionLol.Characters;

/*
	Auto generated by my random js script.
	Additional Information:
		Raw Name: Desirée DeLite
		Formatted Name: DesireDeLite
		Emotion Count: 5
*/
public class DesireDeLite : Character<DesireDeLite.DesireDeLiteEmotions>
{
	public enum DesireDeLiteEmotions
	{
		Cornered = 650,
		Happy = 649,
		Smile = 652,
		Stand = 683,
		Think = 651
	}

	public const string RawName = "Desirée DeLite";

	public DesireDeLite(string username, DesireDeLiteEmotions emotion, string text) : base(username, emotion, text)
	{
	}
}