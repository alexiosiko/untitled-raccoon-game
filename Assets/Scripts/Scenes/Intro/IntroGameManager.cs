using UnityEngine;

public class IntroGameManager : MonoBehaviour
{
    void Start()
    {
        DialogueManager.Singleton.StartNarration("ASDW to walk around.", 6);
		Invoke(nameof(Other), 14);
    }
	void Other()
	{
        DialogueManager.Singleton.StartNarration("Move mouse to look around.");
	}
}
