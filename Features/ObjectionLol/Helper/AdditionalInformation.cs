namespace RandomGameBot.Features.ObjectionLol.Helper;

/// <summary>
/// A JSON-Serializable structure that contains additional information
/// that needs to be included when rendering.
/// </summary>
public class AdditionalInformation
{
	/// <summary>
	/// A list of evidence tags.
	/// </summary>
	public List<EvidenceTag> evidence = new();
	/// <summary>
	/// A list of music changes.
	/// </summary>
	public List<MusicTag> music = new();
}