using System.Collections;
using System.Collections.Generic;
using Unity.FPS.Game;
using UnityEngine;

public class EnemyRun : GAction
{
    private bool run = false;

    public override bool CheckProceduralPrecondition(GameObject agent)
    {
        if (GetComponent<Health>().CurrentHealth <= 25)
        {
            AddEffect("stayAlive", true);
            target = GameObject.FindWithTag("Turret");
            cost = 50;
            return target != null;
        }
        return false;

    }

    
    public override bool IsDone()
    {
        return run;
    }

    public override bool Perform(GameObject agent)
    {
       
        run = true;
        return true;
    }

    public override bool RequiresInRange()
    {
        return true;
    }

    public override void Reset()
    {
        run = false;
        target = null;
    }
}
