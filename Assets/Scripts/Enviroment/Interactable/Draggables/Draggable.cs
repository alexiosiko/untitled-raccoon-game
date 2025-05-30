using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Draggable : Interactable
{
    [Header("Grab Settings")]
    [HideInInspector] public Rigidbody rb;

	[SerializeField] protected bool dragging = false;
	public virtual void StartDrag()
	{
		dragging = true;
	}
	public virtual void StopDrag()
	{
		dragging = false;
	}
    protected virtual void Awake()
	{
		rb = GetComponent<Rigidbody>();
	}
	public override void Action(MonoBehaviour caller)
	{
	}
}