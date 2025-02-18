
using UnityEngine;

public class Grabable : Interactable
{
	public override void Highlight()
	{
		base.Highlight();
	}
	public override void Action()
	{
		base.Action();
		Debug.Log(RaccoonGrab.Singleton.item);
		if (RaccoonGrab.Singleton.item == transform)
			RaccoonGrab.Singleton.Drop();
		else
			RaccoonGrab.Singleton.Grab(transform);
	}
}
