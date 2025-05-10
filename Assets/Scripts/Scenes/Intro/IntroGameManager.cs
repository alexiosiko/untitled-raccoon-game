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
	public void EatApple()
	{
		DialogueManager.Singleton.StartNarration("That was delicious. Let's keep exploring");
	}



}
