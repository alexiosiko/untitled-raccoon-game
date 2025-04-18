using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Draggable : Interactable
{
	public Rigidbody rb; 
	public override void Action(MonoBehaviour caller)
	{
		var machine = caller as RaccoonStateMachine;
		var draggingState =  machine.States[RaccoonState.Dragging] as RaccoonDraggingState;
		draggingState.draggable = this;

		machine.SetState(RaccoonState.Dragging);
	}
	void Awake()
	{
		rb = GetComponent<Rigidbody>();
	}
}