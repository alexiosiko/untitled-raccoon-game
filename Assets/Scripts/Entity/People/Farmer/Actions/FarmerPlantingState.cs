using System.Collections;
using UnityEngine;

public class FarmerPlantingState : BaseState<FarmerState>
{
	private FarmerStateMachine machine;
	public FarmerPlantingState(FarmerStateMachine machine) : base(FarmerState.Planting)
	{
		this.machine = machine;
	}

	public override void EnterState()
	{
		machine.animator.CrossFade("Start Plant", 0.2f);
		randomCoroutine = machine.StartCoroutine(RandomCoroutine());
	}

	public override IEnumerator ExitState()
	{  
		machine.StopCoroutine(randomCoroutine);
		randomCoroutine = null;
		machine.animator.CrossFade("End Plant", 0.2f);
		yield return new WaitForSeconds(1.1f);
		
	}

	public override FarmerState GetNextState()
	{
		return StateKey;
	}
	Coroutine randomCoroutine;
	IEnumerator RandomCoroutine()
	{
		yield return new WaitForSeconds(Random.Range(2, 7));
		machine.SetState(FarmerState.Working);
		randomCoroutine = null;
	}

	public override void UpdateState()
	{
	}
}