namespace RandomGameBot.Features.ObjectionLol.Characters;

/*
	Auto generated by my random js script.
	Additional Information:
		Raw Name: Larry Butz (guard)
		Formatted Name: LarryButzGuard
		Emotion Count: 5
*/
public class LarryButzGuard : Character<LarryButzGuard.LarryButzGuardEmotions>
{
	public enum LarryButzGuardEmotions
	{
		Cornered = 616,
		Cry = 613,
		Happy = 612,
		Think = 614,
		ThumbsUp = 615
	}

	public const string RawName = "Larry Butz (guard)";

	public LarryButzGuard(string username, LarryButzGuardEmotions emotion, string text) : base(username, emotion, text)
	{
	}
}