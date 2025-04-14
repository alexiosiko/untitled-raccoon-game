using UnityEngine;

public abstract class Consumable : Interactable
{
	public override void Action(MonoBehaviour sender)
	{
		#if UNITY_EDITOR
		Debug.Log("Consumable Action", gameObject);
		#endif
	}

}