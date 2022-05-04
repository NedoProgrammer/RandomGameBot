namespace RandomGameBot.Features.ObjectionLol.Characters;

/*
	Auto generated by my random js script.
	Additional Information:
		Raw Name: Sal Manella
		Formatted Name: SalManella
		Emotion Count: 6
*/
public class SalManella : Character<SalManella.SalManellaEmotions>
{
	public enum SalManellaEmotions
	{
		Angry = 348,
		Cornered = 347,
		Damage = 349,
		Shameless = 351,
		Stand = 352,
		Think = 350
	}

	public const string RawName = "Sal Manella";

	public SalManella(string username, SalManellaEmotions emotion, string text) : base(username, emotion, text)
	{
	}
}