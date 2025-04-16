using System.Collections;
using DG.Tweening;
using UnityEngine;

public class RaccoonClimbingState : BaseState<RaccoonState>
{
    RaccoonStateMachine machine;
    bool delay;
    public static float climbingHorizontalDistance = 0.8f;
	[SerializeField] 

    public RaccoonClimbingState(RaccoonStateMachine machine) : base(RaccoonState.Climbing)
    {
        this.machine = machine;
    }

    public override void EnterState()
    {
		machine.climbingCollider.enabled = true;
		delay = true;
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
    }

    public override void UpdateState()
    {
        

        PositionAndRotateClimb();

        if (!delay)
            CheckClimbOver();
    }
	IEnumerator RemoveClimbingParams()
	{
		yield return new WaitForSeconds(1.5f);
		delay = false;
	} 

    public override void ExitState()
    {
		machine.climbingCollider.enabled = false;
        machine.animator.SetBool("Climbing", false);
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
    private void CheckClimbOver()
    {
        Vector3 topCenterAndBack = machine.transform.position + Vector3.up / 1.05f; // This value is also in parentw
        Vector3 halfExtents = Vector3.one / 8f;

        if (!Physics.BoxCast(topCenterAndBack, halfExtents, machine.transform.forward, 
            Quaternion.identity, climbingHorizontalDistance / 1.2f))
        {
            machine.SetState(RaccoonState.ClimbingOver);
        }
    }
    public override RaccoonState GetNextState()
	{

		if (!delay && Input.GetKeyDown(KeyCode.Space) && machine.animator.GetBool("IsGrounded"))

			return RaccoonState.ClimbingCancel;

		return StateKey;
	}
	public static bool CanClimb(RaccoonStateMachine machine)
	{

		return Physics.Raycast( machine.controller.centerOfRaccoon, machine.transform.forward, out RaycastHit hit, RaccoonClimbingState.climbingHorizontalDistance, LayerMask.GetMask("Climbable"));
	
	}
}