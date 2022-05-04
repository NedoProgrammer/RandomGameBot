namespace RandomGameBot.Features.ObjectionLol.Characters;

/*
	Auto generated by my random js script.
	Additional Information:
		Raw Name: Dick Gumshoe
		Formatted Name: DickGumshoe
		Emotion Count: 8
*/
public class DickGumshoe : Character<DickGumshoe.DickGumshoeEmotions>
{
	public enum DickGumshoeEmotions
	{
		Angry = 135,
		Confident = 130,
		Giggle = 134,
		Headscratch = 133,
		Pumped = 137,
		Sad = 132,
		Stand = 131,
		Stare = 136
	}

	public const string RawName = "Dick Gumshoe";

	public DickGumshoe(string username, DickGumshoeEmotions emotion, string text) : base(username, emotion, text)
	{
	}
}