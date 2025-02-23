
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Grabable : Interactable
{
	public bool interactive = false;
	public override void Action()
	{
		if (!interactive)
			return;
		RaccoonGrab.Singleton.GrabOrDrop(this);
	}
	
	void Awake()
	{
		rb = GetComponent<Rigidbody>();	
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
	protected Rigidbody rb;
}


