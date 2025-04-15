using UnityEngine;

public class RaccoonBitingState : BaseState<RaccoonState>
{
    private RaccoonStateMachine machine;

    public RaccoonBitingState(RaccoonStateMachine machine) : base(RaccoonState.Biting)
    {
        this.machine = machine;
    }

    public override void EnterState()
    {
    }

    public override void ExitState()
    {
		machine.animator.CrossFade("Walking", 0.25f);

    }

    public override RaccoonState GetNextState()
    {
		return RaccoonState.Biting;
    }

    public override void UpdateState()
    {
        
    }

 
}