using System.Collections;
using UnityEngine;
using DG.Tweening;

public class FarmerCarryingState : BaseState<FarmerState>
{
	private FarmerStateMachine machine;
	Transform carryItemTransform;
	public FarmerCarryingState(FarmerStateMachine machine) : base(FarmerState.Carrying) => this.machine = machine;
	

	public override IEnumerator EnterState()
	{
		carryItemTransform = machine.destinationTransform;
		var r = machine.destinationTransform.GetComponent<ResettableObject>();
		r.SetGrabState(machine);

		yield return PickUpCoroutine(r);

		MoveToHands();

		yield return new WaitForSeconds(0.5f);
		machine.animator.CrossFade("Carrying", 0.5f);

	}
	IEnumerator PickUpCoroutine(ResettableObject r)
	{

		if (r == null)
		{
			Debug.LogError("ResettableObject is null in PickUpCoroutine!", machine);
			yield break;
		}

		// Set new destination
		Transform target = new GameObject().transform;
		target.position = r.originalPos;
		machine.SetDestination(target);
		
		// Look at
		machine.transform.DOLookAt(carryItemTransform.position, 0.5f, AxisConstraint.Y);

		// Animation
		machine.animator.CrossFade("Carry Start", 0.2f);

		yield return new WaitForSeconds(1f);

	}
	void MoveToHands()
	{
		carryItemTransform.SetParent(machine.carryTransform);
		carryItemTransform.DOLocalMove(Vector3.zero, 1.5f);
		carryItemTransform.DOLocalRotate(Vector3.zero, 1.5f);
	}
	public override IEnumerator ExitState()
	{
		machine.transform.DOLookAt(machine.destinationTransform.position, 0.5f, AxisConstraint.Y);
		machine.animator.CrossFade("Carry Exit", 0.5f);

		if (machine.destinationTransform.name == "TEMP DESTINATION")
			MonoBehaviour.Destroy(machine.destinationTransform.gameObject);

		var r = carryItemTransform.GetComponent<ResettableObject>();

		yield return PickUpCoroutine(r);
		
		r.ResetPositionAndRotation();

		yield return new WaitForSeconds(1f);
		r.SetDropState(machine);
	}

	public override FarmerState GetNextState()
	{
		if (!machine.agent.pathPending && machine.agent.remainingDistance < 1)
			return FarmerState.Walking;

		return StateKey;
	}

	public override void UpdateState()
	{
		machine.FollowDestination();
	}
} 
