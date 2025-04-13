using System.Collections;
using UnityEngine;
public class RaccoonWalkingState : BaseState<RaccoonState>
{
    private RaccoonStateMachine machine;
    private bool startClimbingDelay;

    public RaccoonWalkingState(RaccoonStateMachine machine) : base(RaccoonState.Walking)
    {
        this.machine = machine;
    }

    public override void EnterState()
    {
		startClimbingDelay = true;
		machine.StartCoroutine(RemoveClimbingDelay());

        machine.walkingCollider.enabled = true;
        machine.ResetRotationXZ();
        machine.rb.useGravity = true;
    }

    public override void UpdateState()
    {

    }
	
    public override RaccoonState GetNextState()
	{
		if (!startClimbingDelay && Input.GetKeyDown(KeyCode.Space) && CanClimb())
			return RaccoonState.Climbing;

		if (machine.animator.GetBool("IsGrounded") == false)
			return RaccoonState.ClimbingDown;

		return RaccoonState.Walking; // Stay in this state for now
	}

    public override void ExitState()
    {
    }
	IEnumerator RemoveClimbingDelay()
	{
		yield return new WaitForSeconds(1.5f);
		startClimbingDelay = false;
	} 
	bool CanClimb() => Physics.Raycast(machine.centerOfRaccoon, machine.transform.forward, out RaycastHit hit, machine.climbingHorizontalDistance, LayerMask.GetMask("Climbable"));

}