using System.Collections;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class RaccoonEatingState : BaseState<RaccoonState>
{
	public Interactable consumable;
	RaccoonStateMachine machine;
	public RaccoonEatingState(RaccoonStateMachine machine) : base(RaccoonState.Eating)
	{
		this.machine = machine;
	}

	public override void EnterState()
	{
		machine.animator.CrossFade("Eating", 0.5f);

	}


	public override void ExitState()
	{
		consumable = null;
		machine.animator.CrossFade("Walking", 0.25f);

	}

	public override RaccoonState GetNextState()
	{
		return StateKey;
	}

	public override void UpdateState()
	{
	}
	
}