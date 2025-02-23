using UnityEngine;

public class Test : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
			DialogueManager.Singleton.StartNarration("Your baby is hungry, maybe you should try and find some food nearby ...");
    }
}
