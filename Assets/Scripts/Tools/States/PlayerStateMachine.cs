using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerStateMachine<EState> : MonoBehaviour where EState : Enum
{
	[SerializeField] protected string currentStateName;
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
			StopCoroutine(transitioningStateCoroutine);
			// Debug.Log("Interrupting transition.", this);

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
	protected virtual void OnDestroy()
	{
		if (transitioningStateCoroutine != null)
		{
			StopCoroutine(transitioningStateCoroutine);
			transitioningStateCoroutine = null;
		}
	}

	public Animator animator;

}