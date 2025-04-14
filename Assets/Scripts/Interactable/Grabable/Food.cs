using UnityEngine;

public class Food : Grabable
{
	public override void Action(MonoBehaviour sender)
	{
		base.Action(sender);

		GetComponent<Collider>().enabled = false;
	}

}
