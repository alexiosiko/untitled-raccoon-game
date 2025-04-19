using System.Collections;
using UnityEngine;
public class RaccoonWalkingState : BaseState<RaccoonState>
{
    private RaccoonStateMachine machine;
    private bool delay;

    public RaccoonWalkingState(RaccoonStateMachine machine) : base(RaccoonState.Walking)
    {
        this.machine = machine;
    }

    public override void EnterState()
    {
		// If can instand climb down, don't set walking params cause
		// it won't be smooth
		if (RaccoonClimbingDownState.CanClimbDown(machine, 2))
			machine.SetState(RaccoonState.ClimbingDown);
		else
		{
			delay = true;
			machine.StartCoroutine(RemoveClimbingDelay());
			machine.animator.CrossFade("Walking", 0.25f);
			machine.walkingCollider.enabled = true;
			machine.ResetRotationXZ();
			machine.rb.useGravity = true;
		}
    }

    public override void UpdateState()
    {
		if (!delay && Input.GetKeyDown(KeyCode.Space))
			Interact(machine);
    }
	
	public override RaccoonState GetNextState()
	{
		
		if (!delay && Input.GetKeyDown(KeyCode.Space) && RaccoonClimbingState.CanClimb(machine))
			return RaccoonState.Climbing;

		if (RaccoonClimbingDownState.CanClimbDown(machine) == true)
			return RaccoonState.ClimbingDown;

		// if (machine.animator.GetBool("IsGrounded") == false)
		// 	return RaccoonState.ClimbingDown;

		if (!delay && Input.GetKeyDown(KeyCode.Space))
		{
			if (RaccoonDraggingState.CanDrag(machine) == true)
				return RaccoonState.Dragging;
			
			if (RaccoonGrabbingState.CanGrab(machine) == true)
				return RaccoonState.Grabbing;
				
			if (RaccoonEatingState.CanEat(machine) == true)
				return RaccoonState.Eating;
		}

			

		

		return RaccoonState.Walking; // Stay in this state for now
	}

    public override void ExitState()
    {
    }
	public static void Interact(RaccoonStateMachine machine)
	{
		Vector3 centerEatingSpot = machine.controller.centerOfRaccoon + machine.transform.forward / 3f;
		Debug.DrawLine(machine.controller.centerOfRaccoon, centerEatingSpot, Color.black);
		Collider[] colliders = Physics.OverlapSphere(centerEatingSpot, 0.25f);
		foreach (var c in colliders)
		{
			Interactable i = c.GetComponent<Interactable>();
			if (i)
			{
				i.Action(machine);
				return;
			}
		}
	}
	
	IEnumerator RemoveClimbingDelay()
	{
		yield return new WaitForSeconds(1f);
		delay = false;
	} 
}