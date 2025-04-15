
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
		transform.SetParent(machine.mouthTransform);
		transform.DOLocalMove(Vector3.zero, 0.3f);
		transform.DOLocalRotate(Vector3.zero, 0.5f);
		collider.enabled = false;
		rb.isKinematic = true;
        rb.detectCollisions = false;

	}
	public void SetDropState(MonoBehaviour sender)
	{
		var machine = sender as RaccoonStateMachine;
		transform.SetParent(GameObject.Find("--- ENVIROMENT ---").transform);

		Invoke(nameof(EnableCollider), 0.1f);
		rb.isKinematic = false;
        rb.detectCollisions = true;
		rb.AddForce(machine.transform.forward * 20f);

	}
	void EnableCollider() => collider.enabled = true;
	void Awake()
	{
		rb = GetComponent<Rigidbody>();	
		collider = GetComponent<Collider>();
	}
	new Collider collider;
	protected Rigidbody rb;
}


