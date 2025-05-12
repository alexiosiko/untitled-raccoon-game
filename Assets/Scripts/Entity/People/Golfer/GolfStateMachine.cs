using UnityEngine;

public enum GolfState {
	Walking,
	Chasing,
	Idle,
	Chipping,
	Putting,
}
public class GolfStateMachine : EntityStateMachine<GolfState>
{
	public Transform golfBallTransform;
	protected override void Update()
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
		States.Add(GolfState.Chipping, 		new GolfChippingState(this));
		States.Add(GolfState.Walking, 		new GolfWalkingState(this));

		SetState(GolfState.Walking);


	}
}
