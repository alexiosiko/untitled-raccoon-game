using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine<EState> : MonoBehaviour where EState : Enum
{
	public Dictionary<EState, BaseState<EState>> States = new ();
	protected BaseState<EState> CurrentState;  
	void Start() => CurrentState?.EnterState();
	protected virtual void Update()
	{
		if (CurrentState == null)
			return;
		EState nextStateKey = CurrentState.GetNextState();
		if (nextStateKey.Equals(CurrentState.StateKey))
			CurrentState.UpdateState();
		else if (transitioningStateCoroutine == null)
		{
			// Debug.Log("Transition to " + nextStateKey);
			SetState(nextStateKey);
		}
	}
	public void SetState(EState stateKey, float delay = 0, bool ignoreExitTime = false)
	{

		if (transitioningStateCoroutine != null)
		{
			#if UNITY_EDITOR
			Debug.Log("Interuppting SetState coroutine");
			#endif
			StopCoroutine(transitioningStateCoroutine);

		}
		// Start the new coroutine and store the reference
		transitioningStateCoroutine = StartCoroutine(TransitioningState(stateKey, delay, ignoreExitTime));
		
	}

	IEnumerator TransitioningState(EState stateKey, float delay, bool ignoreExitTime = false)
	{
		yield return new WaitForSeconds(delay);	
		if (ignoreExitTime)
			CurrentState?.ExitState();
		else
			yield return CurrentState?.ExitState();
		
		CurrentState = States[stateKey];
		CurrentState.EnterState();
		
		
		transitioningStateCoroutine = null; // Clear the reference when done
	}
	private Coroutine transitioningStateCoroutine;

}