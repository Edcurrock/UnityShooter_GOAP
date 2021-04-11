using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour, IGOAP
{
    protected float minDist = 2f;
    protected float aggroDist = 10000000f;

    public void ActionsFinished()
    {
  
    }

    public abstract HashSet<KeyValuePair<string, object>> CreateGoalState();

  

    public HashSet<KeyValuePair<string, object>> GetWorldState()
    {
        HashSet<KeyValuePair<string, object>> worldData = new HashSet<KeyValuePair<string, object>>();
        worldData.Add(new KeyValuePair<string, object>("runToPlayer", false));
        worldData.Add(new KeyValuePair<string, object>("evadePlayer", false));
        worldData.Add(new KeyValuePair<string, object>("hitPlayer", false));
        return worldData;
    }

    public bool MoveAgent(GAction nextAction)
    {
        var targetPos = nextAction.target.transform.position;
        float dist = Vector3.Distance(transform.position, targetPos);
        if (dist < aggroDist)
        {
            // GetComponent<NavMeshAgent>().isStopped = false;
            GetComponent<NavMeshAgent>().SetDestination(targetPos);
        }
        // if(dist < minDist)
        // {
        //     GetComponent<NavMeshAgent>().isStopped = true;
        //     nextAction.SetInRange(true);
        //     return true;
        // }
        if(dist < minDist)
        {
            GetComponent<NavMeshAgent>().isStopped = true;
        }
        else
            GetComponent<NavMeshAgent>().isStopped = false;  
        nextAction.SetInRange(true);
        return true;
    }

    public void PlanAborted(GAction aborter)
    {
        
    }

    public void PlanFailed(HashSet<KeyValuePair<string, object>> failedGoal)
    {
      
    }

    public void PlanFound(HashSet<KeyValuePair<string, object>> goal, Queue<GAction> actions)
    {
        
    }

    public override string ToString()
    {
        return base.ToString();
    }
}