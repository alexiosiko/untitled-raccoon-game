using System.Collections;
using UnityEditor;
using UnityEngine;

public class RaccoonDraggingState : BaseState<RaccoonState>
{
    private RaccoonStateMachine machine;
    static Draggable draggable;
	bool delay = false;

	[Header("Drag Settings")]
    float dragForce = 5f; // Force multiplier
	static Vector3 dragPoint;
	Vector3 dragOffset;

    public RaccoonDraggingState(RaccoonStateMachine machine) : base(RaccoonState.Dragging)
    {
        this.machine = machine;
    }

	public override void EnterState()
	{    
		// Calculate offset from object's position to the hit point
		dragOffset = draggable.transform.position - dragPoint;
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
	public static bool CanDrag(RaccoonStateMachine machine)
	{
		Ray ray = new (machine.controller.centerOfRaccoon, machine.transform.forward);
		if (Physics.Raycast(ray, out RaycastHit hit, 1f))
		{
			Draggable d = hit.collider.GetComponent<Draggable>();
			if (d)
			{
				draggable = d;
				dragPoint = hit.point;
				d.Action(machine);
				return true;
			}

		}
		return false;
	}
    public override void UpdateState()
    {
        machine.controller.ForceWalk();
        
        // Calculate target position (maintain original offset from hit point)
        Vector3 targetPosition = dragPoint + dragOffset;
        
        // Get direction to target position
        Vector3 forceDirection = (targetPosition - draggable.transform.position);
        
        // Apply force towards the target position
        draggable.rb.AddForce(forceDirection * dragForce, ForceMode.Acceleration);

        // Visual debug
        Debug.DrawLine(draggable.transform.position, targetPosition, Color.red);
        Debug.DrawRay(dragPoint, Vector3.up * 0.5f, Color.green); // Show drag point
    }

	float dragSpeed = 1f;
	float maxDragSpeed = 1f;
	float dragTurnSpeed = 1f;
}