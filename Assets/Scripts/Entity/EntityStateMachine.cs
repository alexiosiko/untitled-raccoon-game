using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
	This file is the same class as StateMachine, however it enherits EntityController
*/
public abstract class EntityBaseStateMachine<EState> : EntityController where EState : Enum
{
	public Dictionary<EState, BaseState<EState>> States = new ();
	protected BaseState<EState> CurrentState;  
	void Start() => CurrentState?.EnterState();
	protected virtual void Update()
	{

		if (CurrentState == null)
			return;
			
		if (transitioningStateCoroutine != null)
			return;

		EState nextStateKey = CurrentState.GetNextState();
		if (nextStateKey.Equals(CurrentState.StateKey))
			CurrentState.UpdateState();
		else if (transitioningStateCoroutine == null)
		{
			// #if UNITY_EDITOR
			// Debug.Log("Transition to " + nextStateKey);
			// #endif
			SetState(nextStateKey);
		}
	}
	private Coroutine transitioningStateCoroutine;
	public void SetState(EState stateKey, bool ignoreExitTime = false)
	{
		// Interrupt any ongoing transition
		if (transitioningStateCoroutine != null)
		{
			Debug.Log("Already transitioning.");
			return;
		}

		// Begin the new transition
		transitioningStateCoroutine = StartCoroutine(TransitioningState(stateKey, ignoreExitTime));
	}

	IEnumerator TransitioningState(EState stateKey, bool ignoreExitTime = false)
	{
		// If same state
		if (States[stateKey] == CurrentState)
			yield break;

		if (ignoreExitTime)
			CurrentState?.ExitState();
		else
			yield return CurrentState?.ExitState();
		
		CurrentState = States[stateKey];
		yield return CurrentState?.EnterState();
		
		
		transitioningStateCoroutine = null; // Clear the reference when done
	}

}