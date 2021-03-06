namespace RandomGameBot.Features.ObjectionLol.Characters;

/*
	Auto generated by my random js script.
	Additional Information:
		Raw Name: Miles Edgeworth
		Formatted Name: MilesEdgeworth
		Emotion Count: 13
*/
public class MilesEdgeworth : Character<MilesEdgeworth.MilesEdgeworthEmotions>
{
	public enum MilesEdgeworthEmotions
	{
		ArmsCrossed = 272,
		Bow = 5,
		ConfidentShrug = 185,
		ConfidentSmirk = 89,
		Cornered = 7,
		Damage = 6,
		DamageCustom = 407,
		DeskSlam = 90,
		Point = 36,
		Read = 47,
		Smirk = 9,
		Stand = 189,
		Yell = 8
	}

	public const string RawName = "Miles Edgeworth";

	public MilesEdgeworth(string username, MilesEdgeworthEmotions emotion, string text) : base(username, emotion, text)
	{
	}
}