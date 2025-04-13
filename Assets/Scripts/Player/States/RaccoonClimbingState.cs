using System.Collections;
using DG.Tweening;
using UnityEngine;

public class RaccoonClimbingState : BaseState<RaccoonState>
{
    private RaccoonStateMachine machine;
    private bool startClimbingDelay;
    public static float climbingHorizontalDistance = 0.8f;

    public RaccoonClimbingState(RaccoonStateMachine machine) : base(RaccoonState.Climbing)
    {
        this.machine = machine;
    }

    public override void EnterState()
    {
		startClimbingDelay = true;
		machine.StartCoroutine(RemoveClimbingParams());
        machine.animator.SetBool("Climbing", true);
        machine.walkingCollider.enabled = false;
        machine.rb.useGravity = false;
        
        // Hover from climbable
        if (Physics.Raycast(machine.centerOfRaccoon, machine.transform.forward, 
            out RaycastHit hit, climbingHorizontalDistance, LayerMask.GetMask("Climbable")))
        {
			// Place position
            Vector3 newPos = hit.point + Vector3.down / 10f + hit.normal / 4f;
            machine.transform.DOMove(newPos, 1.2f);
        }
    }

    public override void UpdateState()
    {
        

        PositionAndRotateClimb();

        if (!startClimbingDelay)
            CheckClimbOver();
    }
	IEnumerator RemoveClimbingParams()
	{
		yield return new WaitForSeconds(1.5f);
		startClimbingDelay = false;
	} 

    public override void ExitState()
    {
        machine.animator.SetBool("Climbing", false);
    }
    private void PositionAndRotateClimb()
    {
        if (Physics.Raycast(machine.centerOfRaccoon, machine.transform.forward, 
            out RaycastHit hit, climbingHorizontalDistance, LayerMask.GetMask("Climbable")))
        {
            Vector3 forwardDirection = -hit.normal;
            Quaternion targetRotation = Quaternion.LookRotation(forwardDirection);
            machine.transform.rotation = Quaternion.Slerp(
                machine.transform.rotation, 
                targetRotation, 
                Time.deltaTime * 2f
            );
        }
    }
    private void CheckClimbOver()
    {
        Vector3 topCenterAndBack = machine.transform.position + Vector3.up / 1.05f; // This value is also in parentw
        Vector3 halfExtents = Vector3.one / 8f;

        if (!Physics.BoxCast(topCenterAndBack, halfExtents, machine.transform.forward, 
            Quaternion.identity, climbingHorizontalDistance / 1.2f))
        {
            machine.TransitionToState(RaccoonState.ClimbingOver);
        }
    }
    public override RaccoonState GetNextState()
	{
		if (!startClimbingDelay && Input.GetKeyDown(KeyCode.Space) && machine.animator.GetBool("IsGrounded"))
			return RaccoonState.ClimbingDown;

		return StateKey;
	}
}