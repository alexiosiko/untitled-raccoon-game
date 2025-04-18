using System.Collections;
using UnityEngine;

public class RaccoonDraggingState : BaseState<RaccoonState>
{
    private RaccoonStateMachine machine;
    public Draggable draggable;
	bool delay = false;

	[Header("Drag Settings")]
    float dragForce = 5f; // Force multiplier
	float offsetY;

    public RaccoonDraggingState(RaccoonStateMachine machine) : base(RaccoonState.Dragging)
    {
        this.machine = machine;
    }

    public override void EnterState()
    {
		offsetY = machine.controller.centerOfRaccoon.y - draggable.transform.position.y;
		delay = true;
		machine.StartCoroutine(RemoveDelay());
    }

    public override void ExitState()
    {
        draggable = null;
        machine.animator.CrossFade("Walking", 0.25f);
    }
	IEnumerator RemoveDelay()
	{
		yield return new WaitForSeconds(0.5f);
		delay = false;
	}
    public override RaccoonState GetNextState()
    {
        // Transition to Walking when releasing drag
        if (!delay && Input.GetKeyDown(KeyCode.Space)) // Use your input method
            return RaccoonState.Walking;
        return RaccoonState.Dragging;
    }

    public override void UpdateState()
    {
		machine.controller.ForceWalk();
		Vector3 dragToPosition = machine.controller.centerOfRaccoon + machine.transform.forward * 1f;
		dragToPosition.y += offsetY; 
		Vector3 directionWithDistance = dragToPosition - draggable.transform.position;	

		// Apply force (stronger when farther away)
        draggable.rb.AddForce(directionWithDistance * dragForce, ForceMode.Acceleration);



		#if UNITY_EDITOR
		Debug.DrawLine(machine.controller.centerOfRaccoon, dragToPosition, Color.yellow);
		#endif
    }

	float dragSpeed = 1f;
	float maxDragSpeed = 1f;
	float dragTurnSpeed = 1f;
}