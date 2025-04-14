
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Grabable : Interactable
{
	public bool interactive = true;
	public override void Action(MonoBehaviour sender)
	{
		// if (!interactive)
		// 	return;

		Debug.Log(sender);
		sender.SendMessage(nameof(RaccoonStateMachine.SetGrabbingState), this);
	}
	
	
	public void SetGrabState(MonoBehaviour sender)
	{
		var machine = sender as RaccoonStateMachine;
		machine.transform.SetParent(machine.mouthTransform);
		machine.transform.DOLocalMove(Vector3.zero, 0.3f);
		collider.enabled = false;
		rb.isKinematic = true;
        rb.detectCollisions = false;

	}
	public void SetDropState()
	{
		collider.enabled = true;
		rb.isKinematic = false;
        rb.detectCollisions = true;
		transform.SetParent(GameObject.Find("--- ENVIROMENT ---").transform);

	}
	void Awake()
	{
		rb = GetComponent<Rigidbody>();	
		collider = GetComponent<Collider>();
	}
	new Collider collider;
	protected Rigidbody rb;
}


