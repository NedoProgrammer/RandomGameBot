namespace RandomGameBot.Features.ObjectionLol.Characters;

/*
	Auto generated by my random js script.
	Additional Information:
		Raw Name: Matt Engarde
		Formatted Name: MattEngarde
		Emotion Count: 8
*/
public class MattEngarde : Character<MattEngarde.MattEngardeEmotions>
{
	public enum MattEngardeEmotions
	{
		Breakdown = 254,
		Call = 253,
		Cornered = 256,
		Evil = 258,
		Laugh = 257,
		OnCall = 259,
		Stand = 255,
		Uncertain = 260
	}

	public const string RawName = "Matt Engarde";

	public MattEngarde(string username, MattEngardeEmotions emotion, string text) : base(username, emotion, text)
	{
	}
}