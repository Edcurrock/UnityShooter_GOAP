using System.Collections;
using System.Collections.Generic;
using Unity.FPS.Game;
using UnityEngine;

public class EnemyAttack : GAction
{
    [SerializeField] Animator animator;
    private bool attacked = false;
    const string k_AnimAttackParameter = "Attack";
    public EnemyAttack()
    {
        AddPrecondition("runToPlayer", false);
        AddEffect("runToPlayer", true);
        cost = 100f;
    }
    public override bool CheckProceduralPrecondition(GameObject agent)
    {
        target = GameObject.FindWithTag("Player");
        return target != null;
    }

    public override bool IsDone()
    {
        return attacked;
    }

    public override bool Perform(GameObject agent)
    {
      // OnAttack();
       attacked = true;
       return true;
        
    }


    void OnAttack()
    {
        animator.SetTrigger(k_AnimAttackParameter);
    }
    public override bool RequiresInRange()
    {
        return true;
    }

    public override void Reset()
    {
        attacked = false;
        target = null;
    }
}
