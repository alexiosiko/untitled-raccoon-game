using System.Collections;
using UnityEngine;

public enum FarmerState {
	Wonder,
	Angry,
	Cheering,
	Carrying,

	Planting,

	Sitting,

	Idle,
	Working,

	Chasing,
}
public class FarmerStateMachine : EntityBaseStateMachine<FarmerState>
{
	[SerializeField] Transform[] plants;
	[SerializeField] string currentStateName;
	[SerializeField] public Transform carryTransform;
	void LateUpdate()
    {
		base.Update();
		currentStateName = CurrentState?.ToString();

    }
	

	protected override void Awake()
	{
		base.Awake();

		
		// States.Add(FarmerState.Wonder, 			new FarmerWonderState(this));
		// States.Add(FarmerState.Angry, 			new FarmerAngryState(this));
		// States.Add(FarmerState.Cheering, 		new FarmerCheeringState(this));
		States.Add(FarmerState.Carrying, 		new FarmerCarryingState(this));
		States.Add(FarmerState.Sitting, 		new FarmerSittingState(this));
		States.Add(FarmerState.Working, 		new FarmerWorkingState(this));
		States.Add(FarmerState.Planting, 		new FarmerPlantingState(this));
		States.Add(FarmerState.Idle, 			new FarmerIdleState(this));
		States.Add(FarmerState.Chasing, 		new FarmerChasingState(this));

		SetState(FarmerState.Working);


	}



	public void SetRandomPlantDesination()
	{
		destinationTransform = plants[Random.Range(0, plants.Length)];
	}
	[SerializeField] public ResettableObject[] resettableObjects;
}
