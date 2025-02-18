using UnityEngine;

public class Interactable : MonoBehaviour
{
	public virtual void Highlight() {}
	public virtual void Action() {
		print("Action on: " + this);
	}
}