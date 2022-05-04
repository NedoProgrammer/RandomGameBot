namespace RandomGameBot.Features.ObjectionLol.Characters;

/*
	Auto generated by my random js script.
	Additional Information:
		Raw Name: Judge's Brother
		Formatted Name: JudgesBrother
		Emotion Count: 6
*/
public class JudgesBrother : Character<JudgesBrother.JudgesBrotherEmotions>
{
	public enum JudgesBrotherEmotions
	{
		EyesClosed = 127,
		Headshake = 125,
		Negative = 128,
		Positive = 126,
		Stand = 605,
		Surprised = 187
	}

	public const string RawName = "Judge's Brother";

	public JudgesBrother(string username, JudgesBrotherEmotions emotion, string text) : base(username, emotion, text)
	{
	}
}