using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.SocialPlatforms.Impl;

public class FarmerPlantingState : BaseState<FarmerState>
{
	private FarmerStateMachine machine;
	public FarmerPlantingState(FarmerStateMachine machine) : base(FarmerState.Planting)
	{
		this.machine = machine;
	}

	public override IEnumerator EnterState()
	{
		machine.animator.CrossFade("Plant Start", 0.2f);
		randomCoroutine = machine.StartCoroutine(RandomCoroutine());
		machine.StartCoroutine(RandomCoroutine());
		yield return null;
		
	}

	public override IEnumerator ExitState()
	{  
		if (randomCoroutine != null)
			machine.StopCoroutine(randomCoroutine);

		machine.animator.CrossFade("Plant Exit", 0.2f);
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
		SetRandomPlantDesination(machine);
		machine.SetState(FarmerState.Walking);
		randomCoroutine = null;
		// randomCoroutine = null;
	}
	public static void SetRandomPlantDesination(FarmerStateMachine machine)
	{
		machine.SetDestination(machine.plants[Random.Range(0, machine.plants.Length)]);
		// _ = machine.SetDestinationAsync();
	}
	public override void UpdateState()
	{
	}
}