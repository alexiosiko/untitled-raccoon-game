
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Grabable : Interactable
{
	public bool interactive = true;
	public override void Action(MonoBehaviour sender)
	{
		
	}
	
	
	public virtual void SetGrabState(MonoBehaviour sender)
	{
		col.enabled = false;
		rb.isKinematic = true;
        rb.detectCollisions = false;
	}
	public virtual void SetDropState(MonoBehaviour sender)
	{
		col.enabled = true;
		CancelInvoke();
		Invoke(nameof(UnIsKinematic), 2);
        rb.detectCollisions = true;
		transform.SetParent(GameObject.Find("--- ENVIROMENT ---").transform);

	}
	public void UnIsKinematic() => rb.isKinematic = false;
	protected virtual void Awake()
	{
		rb = GetComponent<Rigidbody>();	
		col = GetComponent<Collider>();
	}
  	[SerializeField] protected Collider col;
	[SerializeField] protected Rigidbody rb;
}


