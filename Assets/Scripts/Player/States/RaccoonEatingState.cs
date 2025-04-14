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
 	    consumable.transform.SetParent(machine.mouthTransform);
		consumable.transform.DOLocalMove(Vector3.zero, 0.5f);
	}


	public override void ExitState()
	{
		consumable = null;
	}

	public override RaccoonState GetNextState()
	{
		return StateKey;
	}

	public override void UpdateState()
	{
	}
	
}