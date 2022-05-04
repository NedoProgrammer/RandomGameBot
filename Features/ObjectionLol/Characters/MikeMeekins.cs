namespace RandomGameBot.Features.ObjectionLol.Characters;

/*
	Auto generated by my random js script.
	Additional Information:
		Raw Name: Mike Meekins
		Formatted Name: MikeMeekins
		Emotion Count: 8
*/
public class MikeMeekins : Character<MikeMeekins.MikeMeekinsEmotions>
{
	public enum MikeMeekinsEmotions
	{
		Damage = 394,
		Megaphone = 398,
		Pumped = 391,
		Sad = 392,
		Salute = 397,
		Stand = 393,
		Stare = 395,
		Think = 396
	}

	public const string RawName = "Mike Meekins";

	public MikeMeekins(string username, MikeMeekinsEmotions emotion, string text) : base(username, emotion, text)
	{
	}
}