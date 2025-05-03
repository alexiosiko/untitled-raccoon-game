using System.Collections;
using UnityEngine;

public enum FarmerState {
	Wonder,
	Angry,
	Cheering,

	Planting,

	Sitting,

	Idle,
	Working,

	Running,
}
public class FarmerStateMachine : EntityBaseStateMachine<FarmerState>
{
	[SerializeField] Transform[] plants;
	[SerializeField] string currentStateName;
	protected override void Update()
    {
		base.Update();
		currentStateName = CurrentState?.ToString();

		if (Input.GetKeyDown(KeyCode.I))
			SetState(FarmerState.Working);

		if (Input.GetKeyDown(KeyCode.K))
			SetState(FarmerState.Sitting);

		if (Input.GetKeyDown(KeyCode.M))
			SetState(FarmerState.Idle);

		if (Input.GetKeyDown(KeyCode.P))
			SetState(FarmerState.Planting);

		if (Input.GetKeyDown(KeyCode.T))
			SetState(FarmerState.Running);

    }
	

	protected override void Awake()
	{
		base.Awake();

		
		States.Add(FarmerState.Wonder, 			new FarmerWonderState(this));
		States.Add(FarmerState.Angry, 			new FarmerAngryState(this));
		States.Add(FarmerState.Cheering, 		new FarmerCheeringState(this));
		States.Add(FarmerState.Sitting, 		new FarmerSittingState(this));
		States.Add(FarmerState.Working, 		new FarmerWorkingState(this));
		States.Add(FarmerState.Planting, 		new FarmerPlantingState(this));
		States.Add(FarmerState.Idle, 			new FarmerIdleState(this));
		States.Add(FarmerState.Running, 		new FarmerRunningState(this));

		SetState(FarmerState.Working);
		
	}

	public void SetRandomPlantDesination()
	{
		destinationTransform = plants[Random.Range(0, plants.Length)];
	}
}
