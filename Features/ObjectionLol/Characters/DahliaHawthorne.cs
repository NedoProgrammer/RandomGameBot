namespace RandomGameBot.Features.ObjectionLol.Characters;

/*
	Auto generated by my random js script.
	Additional Information:
		Raw Name: Dahlia Hawthorne
		Formatted Name: DahliaHawthorne
		Emotion Count: 9
*/
public class DahliaHawthorne : Character<DahliaHawthorne.DahliaHawthorneEmotions>
{
	public enum DahliaHawthorneEmotions
	{
		Angry = 168,
		Cry = 164,
		Damage = 167,
		HairPlay = 170,
		Happy = 165,
		LookAway = 169,
		Nervous = 171,
		Stand = 166,
		Uncertain = 172
	}

	public const string RawName = "Dahlia Hawthorne";

	public DahliaHawthorne(string username, DahliaHawthorneEmotions emotion, string text) : base(username, emotion,
		text)
	{
	}
}