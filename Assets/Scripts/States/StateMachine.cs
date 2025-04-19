using System;
using System.Collections;
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
		if (setStateCoroutine != null)
			StopCoroutine(setStateCoroutine);
		CurrentState.EnterState();
		IsTransitioningState = false;
	}
	private Coroutine setStateCoroutine;

	public void SetState(EState stateKey, float delay)
	{
		// Stop the specific coroutine if it's running
		if (setStateCoroutine != null)
			StopCoroutine(setStateCoroutine);
		
		// Start the new coroutine and store the reference
		setStateCoroutine = StartCoroutine(SetStateCoroutine(stateKey, delay));
	}

	IEnumerator SetStateCoroutine(EState stateKey, float delay)
	{
		yield return new WaitForSeconds(delay);
		SetState(stateKey);
		setStateCoroutine = null; // Clear the reference when done
}
	public abstract void Awake();

}