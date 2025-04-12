using UnityEngine;

public class RaccoonFallingState : BaseState<RaccoonState>
{
    private RaccoonStateMachine machine;
    public RaccoonFallingState(RaccoonStateMachine machine) : base(RaccoonState.Falling)
    {
        this.machine = machine;
    }

    public override void EnterState()
    {
		Debug.Log(machine.animator.applyRootMotion);
		
        machine.walkingCollider.enabled = false;
		// machine.animator.applyRootMotion = false;
		Debug.Log(machine.animator.applyRootMotion);
		// machine.ForwardForce();
    }

    public override void UpdateState()
    {

    }

    public override RaccoonState GetNextState()
	{
        if (machine.IsGrounded())
          	return RaccoonState.Walking;
		else
			return StateKey;
	}



    public override void ExitState()
    {
        machine.walkingCollider.enabled = true;
		machine.animator.applyRootMotion = true;
	
	}
}