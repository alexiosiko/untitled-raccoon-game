using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine<EState> : MonoBehaviour where EState : Enum
{
	public Dictionary<EState, BaseState<EState>> States = new ();
	protected BaseState<EState> CurrentState;  
	protected bool IsTransitioningState = false;
	void Start() => CurrentState.EnterState();
	protected virtual void Update()
	{
		if (CurrentState == null)
			return;
		EState nextStateKey = CurrentState.GetNextState();
		if (nextStateKey.Equals(CurrentState.StateKey))
		{
			CurrentState.UpdateState();
		}
		else if (!IsTransitioningState)
		{
			Debug.Log("Transition to " + nextStateKey);
			SetState(nextStateKey);
		}
	}
	public void SetState(EState stateKey)
	{
		IsTransitioningState = true;
		CurrentState?.ExitState();
		CurrentState = States[stateKey];
		CurrentState.EnterState();
		IsTransitioningState = false;
	}
	public abstract void Awake();

}