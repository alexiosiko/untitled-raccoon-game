using System.Collections;
using DG.Tweening;
using UnityEngine;

public class RaccoonClimbingState : BaseState<RaccoonState>
{
    RaccoonStateMachine machine;
    bool delay;
	bool climbOverDelay;
    public static float climbingHorizontalDistance = 0.8f;
	[SerializeField] 

    public RaccoonClimbingState(RaccoonStateMachine machine) : base(RaccoonState.Climbing)
    {
        this.machine = machine;
    }

    public override IEnumerator EnterState()
    {
		// machine.climbingCollider.enabled = true;
		delay = true;
		climbOverDelay = true;
		machine.StartCoroutine(RemoveClimbingParams());
		machine.StartCoroutine(RemoveClimbingParams());
        machine.animator.SetBool("Climbing", true);
        machine.walkingCollider.enabled = false;
        machine.rb.useGravity = false;
        
        // Hover from climbable
        if (Physics.Raycast(machine.controller.centerOfRaccoon, machine.transform.forward, 
            out RaycastHit hit, climbingHorizontalDistance, LayerMask.GetMask("Climbable")))
        {
			// Place position
            Vector3 newPos = hit.point + Vector3.down / 10f + hit.normal / 4f;
            machine.transform.DOMove(newPos, 0.7f);
        }
		yield return null;
    }

    public override void UpdateState()
    {
        PositionAndRotateClimb();
    }
	IEnumerator RemoveClimbingParams()
	{
		yield return new WaitForSeconds(0.5f);
		climbOverDelay = false;
		yield return new WaitForSeconds(1f);
		delay = false;
	} 

    public override IEnumerator ExitState()
    {
		// machine.climbingCollider.enabled = false;
        machine.animator.SetBool("Climbing", false);
		yield return null;
    }
    private void PositionAndRotateClimb()
    {
        if (Physics.Raycast(machine.controller.centerOfRaccoon, machine.transform.forward, 
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

    public override RaccoonState GetNextState()
	{
		Vector3 topCenterAndBack = machine.transform.position + Vector3.up / 1.05f; // This value is also in parentw
        Vector3 halfExtents = Vector3.one / 8f;

        if (!climbOverDelay &&  !Physics.BoxCast(topCenterAndBack, halfExtents, machine.transform.forward, 
            Quaternion.identity, climbingHorizontalDistance / 1.2f))
            return RaccoonState.ClimbingOver;

		if (!delay && Input.GetKeyDown(KeyCode.Space) && machine.animator.GetBool("IsGrounded"))
			return RaccoonState.ClimbingCancel;

		return StateKey;
	}
	public static bool CanClimb(RaccoonStateMachine machine) => Physics.Raycast( machine.controller.centerOfRaccoon, machine.transform.forward, RaccoonClimbingState.climbingHorizontalDistance, LayerMask.GetMask("Climbable"));
}