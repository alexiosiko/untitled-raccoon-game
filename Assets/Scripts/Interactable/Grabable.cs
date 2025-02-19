
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
		RaccoonGrab.Singleton.GrabOrDrop(this);
	}
	public void SetGrabState()
	{
		rb.isKinematic = true;
        rb.detectCollisions = false;
	}
	public void SetDropState()
	{
		rb.isKinematic = false;
        rb.detectCollisions = true;
	}
	void Awake()
	{
		rb = GetComponent<Rigidbody>();	
	}
	Rigidbody rb;
}


