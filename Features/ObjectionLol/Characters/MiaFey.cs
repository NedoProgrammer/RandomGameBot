namespace RandomGameBot.Features.ObjectionLol.Characters;

/*
	Auto generated by my random js script.
	Additional Information:
		Raw Name: Mia Fey
		Formatted Name: MiaFey
		Emotion Count: 9
*/
public class MiaFey : Character<MiaFey.MiaFeyEmotions>
{
	public enum MiaFeyEmotions
	{
		Confident = 273,
		Cornered = 38,
		Damage = 15,
		DamageCustom = 17,
		DeskSlam = 16,
		Point = 409,
		Stand = 244,
		Thinking = 39,
		Yell = 18
	}

	public const string RawName = "Mia Fey";

	public MiaFey(string username, MiaFeyEmotions emotion, string text) : base(username, emotion, text)
	{
	}
}