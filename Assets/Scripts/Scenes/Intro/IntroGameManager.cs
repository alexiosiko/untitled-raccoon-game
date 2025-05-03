using UnityEngine;

public class IntroGameManager : MonoBehaviour
{
    void Start()
    {
        DialogueManager.Singleton.StartNarration("ASDW to walk around.", 3);
    }
	public void EatApple()
	{
		DialogueManager.Singleton.StartNarration("That was delicious. Let's keep exploring");
	}



}
