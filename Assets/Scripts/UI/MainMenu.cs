using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	public void LoadScene(string scene)
	{
		SceneManager.LoadScene(scene);
	}
}
