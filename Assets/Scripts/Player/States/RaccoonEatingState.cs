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
	public static bool CanEat(RaccoonStateMachine machine)
	{
		Vector3 centerEatingSpot = machine.controller.centerOfRaccoon + machine.transform.forward / 3f;
		Debug.DrawLine(machine.controller.centerOfRaccoon, centerEatingSpot, Color.black);
		Collider[] colliders = Physics.OverlapSphere(centerEatingSpot, 0.25f);
		foreach (var c in colliders)
		{
			Consumable i = c.GetComponent<Consumable>();
			if (i)
			{
				i.Action(machine);
				return true;
			}
		}
		return false;
	}
	public override void UpdateState()
	{
	}
	
}