using System;
using System.Collections;

public abstract class BaseState<EState> where EState : Enum
{
	public BaseState(EState key) => StateKey = key;
	public EState StateKey { get; private set; }
	public abstract IEnumerator EnterState();	
	public abstract void UpdateState();	
	public abstract EState GetNextState();
	public abstract IEnumerator ExitState();	
}