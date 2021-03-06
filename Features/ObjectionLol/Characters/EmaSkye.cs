namespace RandomGameBot.Features.ObjectionLol.Characters;

/*
	Auto generated by my random js script.
	Additional Information:
		Raw Name: Ema Skye
		Formatted Name: EmaSkye
		Emotion Count: 4
*/
public class EmaSkye : Character<EmaSkye.EmaSkyeEmotions>
{
	public enum EmaSkyeEmotions
	{
		Angry = 354,
		Determined = 353,
		Stand = 355,
		Think = 356
	}

	public const string RawName = "Ema Skye";

	public EmaSkye(string username, EmaSkyeEmotions emotion, string text) : base(username, emotion, text)
	{
	}
}