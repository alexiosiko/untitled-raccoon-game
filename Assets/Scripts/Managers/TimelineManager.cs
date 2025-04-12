using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Playables;

public enum TimelineState {
	idle,
	waitingToJumpOutSewer,
}
public class TimelineManager : MonoBehaviour
{
	[SerializeField] TimelineState state;
	void Start()
	{
		SetState(TimelineState.waitingToJumpOutSewer);	
	}
	public void SetState(TimelineState state)
	{
		ExitState();
		this.state = state;
		EnterState();
	}
	void EnterState()
	{

		switch (state)
		{
			case TimelineState.waitingToJumpOutSewer:
				DialogueManager.Singleton.StartNarration("Press SPACE to get out of trash can");
				break;
		}
	}
	void Update()
	{
		switch (state)
		{
			case TimelineState.waitingToJumpOutSewer:
				if (Input.GetKeyDown(KeyCode.Space))
					SetState(TimelineState.idle);
				break;
		}
	}
	void ExitState()
	{
		switch (state)
		{
			case TimelineState.waitingToJumpOutSewer:
				CameraController.freeze = false;
				PlayDirector("Raccoon Climb Out of Sewer");
				break;
		}
	}
	void PlayDirector(string timelineName)
	{
		PlayableDirector[] directors = GetComponents<PlayableDirector>();
		foreach (var d in directors)
		{
			if (d.playableAsset.name == timelineName)
			{
				d.Play();
				return;
			}
		}
		Debug.LogError($"Could not find timeline: ${timelineName}");
	}
	public static TimelineManager Singleton;
	void Awake() => Singleton = this;
}
