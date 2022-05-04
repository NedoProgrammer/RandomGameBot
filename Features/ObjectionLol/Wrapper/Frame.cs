namespace RandomGameBot.Features.ObjectionLol.Wrapper;

public class Frame
{
	public int bubbleType = 0;
	public int? characterId = null;
	public bool doNotTalk = false;
	public object? filter = null;
	public object? flipped = null;
	public List<object> frameActions;
	public List<object> frameFades;
	public bool goNext = false;
	public int id = -1;
	public int iid;
	public bool mergeNext = false;
	public int? pairId = null;
	public int? pairPoseId = null;
	public int? popupId = null;
	public bool poseAnimation = true;
	public int poseId;
	public string text;
	public object? transition = null;
	public string username;
}