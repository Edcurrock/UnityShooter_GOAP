using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public sealed class GAgent : MonoBehaviour {

	private FSM stateMachine;
	private FSM.FSMState idleState;
	private FSM.FSMState moveToState;
	private FSM.FSMState performActionState;

	private HashSet<GAction> availableActions;
	private Queue<GAction> currentActions;
	private IGOAP dataProvider;
	private GPlanner planner;


	// Use this for initialization
	void Start () 
	{
		stateMachine = new FSM();
		availableActions = new HashSet<GAction> ();
		currentActions = new Queue<GAction> ();
		planner = new GPlanner();
		FindDataProvider ();
		CreateIdleState ();
		CreateMoveToState ();
		CreatePerformActionState ();
		stateMachine.PushState (idleState);
		LoadActions ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		stateMachine.Update(this.gameObject);
	}

	public void AddAction(GAction action)
	{
		availableActions.Add(action);
	}

	public GAction GetAction(Type action)
	{
		foreach (GAction currAction in availableActions) 
		{
			if(currAction.GetType().Equals(action))
			{
				return currAction;
			}
		}
		return null;
	}

	public void RemoveAction(GAction action) => availableActions.Remove(action);
	

	private bool HasActionPlan() =>  currentActions.Count > 0;
	

	private void CreateIdleState()
	{
		idleState = (fsm, obj) => 
		{

			HashSet<KeyValuePair<string, object>> worldState = dataProvider.GetWorldState();
			HashSet<KeyValuePair<string, object>> goal = dataProvider.CreateGoalState();

			Queue<GAction> plan = planner.Plan(gameObject, availableActions, worldState, goal);
			if (plan != null) 
			{
				currentActions = plan;
				dataProvider.PlanFound(goal, plan);

				fsm.PopState();
				fsm.PushState(performActionState);
			} 
			else 
			{
				dataProvider.PlanFailed(goal);
				fsm.PopState();
				fsm.PushState(idleState);
			}
		};
	}

	private void CreateMoveToState(){
		moveToState = (fsm, gameObject) => {

			GAction action = currentActions.Peek ();
			if (action.RequiresInRange() && action.target == null) {
				fsm.PopState();
				fsm.PopState();
				fsm.PushState(idleState);
				return;
			}

			if (dataProvider.MoveAgent (action)) {
				fsm.PopState ();
			}

		};
	}

	private void CreatePerformActionState()
	{
		performActionState = (fsm, obj) => {

			if (!HasActionPlan()) 
			{
				fsm.PopState();
				fsm.PushState(idleState);
				dataProvider.ActionsFinished();
				return;
			}

			GAction action = currentActions.Peek();
			if (action.IsDone()) 
			{
				currentActions.Dequeue ();
			}

			if (HasActionPlan()) 
			{
				action = currentActions.Peek();
				bool inRange = action.RequiresInRange() ? action.IsInRange() : true;

				if (inRange) 
				{
					bool success = action.Perform (obj);
					if (!success) {
						fsm.PopState ();
						fsm.PushState(idleState);
						CreateIdleState();
						dataProvider.PlanAborted(action);
					} 
				} 
				else 
				{
					fsm.PushState (moveToState);
				}
			} 
			else 
			{
				fsm.PopState ();
				fsm.PushState (idleState);
				dataProvider.ActionsFinished();
			}
		};
	}

	private void FindDataProvider()
	{
		foreach (Component comp in gameObject.GetComponents(typeof(Component))) {
			if (typeof(IGOAP).IsAssignableFrom(comp.GetType ())) 
			{
				dataProvider = (IGOAP)comp;
				return;
			}
		}
	}

	private void LoadActions()
	{
		GAction[] actions = gameObject.GetComponents<GAction>();
		foreach (GAction a in actions) 
		{
			availableActions.Add(a);
		}
	}
}
