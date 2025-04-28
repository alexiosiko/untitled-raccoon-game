using UnityEngine;

public class IntroGameManager : MonoBehaviour
{
    void Start()
    {
		
        DialogueManager.Singleton.StartNarration("ASDW to walk around.", 3);
    }


}
