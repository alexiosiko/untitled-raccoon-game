using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
public enum FarmerState {
	Wonder,
	Angry,
	Cheering,
	Carrying,

	Planting,

	Sitting,

	Idle,
	Walking,

	Chasing,
}
public class FarmerStateMachine : EntityStateMachine<FarmerState>
{
	[SerializeField] public Transform[] plants;
	[SerializeField] public Transform carryTransform;
	void LateUpdate()
    {
		base.Update();
		currentStateName = CurrentState?.ToString();
		// if (Input.GetKeyDown(KeyCode.N))
		// {
		// 	FarmerPlantingState.SetRandomPlantDesination(this);
		// 	SetState(FarmerState.Walking);
		// }
    }
	

	protected override void Awake()
	{
		base.Awake();

		
		// States.Add(FarmerState.Wonder, 			new FarmerWonderState(this));
		// States.Add(FarmerState.Angry, 			new FarmerAngryState(this));
		// States.Add(FarmerState.Cheering, 		new FarmerCheeringState(this));
		States.Add(FarmerState.Carrying, 		new FarmerCarryingState(this));
		States.Add(FarmerState.Sitting, 		new FarmerSittingState(this));
		States.Add(FarmerState.Walking, 		new FarmerWalkingState(this));
		States.Add(FarmerState.Planting, 		new FarmerPlantingState(this));
		States.Add(FarmerState.Idle, 			new FarmerIdleState(this));
		States.Add(FarmerState.Chasing, 		new FarmerChasingState(this));

		SetState(FarmerState.Walking);


	}




	[SerializeField] public ResettableObject[] resettableObjects;
}
