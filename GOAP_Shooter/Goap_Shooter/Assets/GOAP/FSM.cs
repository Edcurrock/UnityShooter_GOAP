using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class FSM {

	private Stack<FSMState> stackOfState = new Stack<FSMState>();

	public delegate void FSMState(FSM fsm, GameObject obj);

	public void Update(GameObject obj)
	{
		if (stackOfState.Peek() != null) 
		{
            stackOfState.Peek().Invoke (this, obj);
		}
	}

	public void PushState(FSMState state)
	{
        stackOfState.Push(state);
	}

	public void PopState()
	{
        stackOfState.Pop();
	}
}
