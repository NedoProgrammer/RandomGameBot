namespace RandomGameBot.Features.ObjectionLol.Characters;

/*
	Auto generated by my random js script.
	Additional Information:
		Raw Name: Gallery
		Formatted Name: Gallery
		Emotion Count: 3
*/
public class Gallery : Character<Gallery.GalleryEmotions>
{
	public enum GalleryEmotions
	{
		EmptyAudience = 216,
		Idle = 215,
		Talk = 540
	}

	public const string RawName = "Gallery";

	public Gallery(string username, GalleryEmotions emotion, string text) : base(username, emotion, text)
	{
	}
}