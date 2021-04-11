using System.Collections;
using System.Collections.Generic;
using Unity.FPS.Game;
using UnityEngine;
using  Unity.FPS.AI;
public class HitPlayer : GAction
{
    const string k_AnimAttackParameter = "Attack";
    const string k_AnimOnDamagedParameter = "OnDamaged";
    private bool hit = false;

    [Tooltip("The random hit damage effects")]
    public ParticleSystem[] RandomHitSparks;

    [SerializeField] EnemyController m_EnemyController;
    public Animator Animator;

    public HitPlayer()
    {
       
        cost = 75f;
    }
    public override bool CheckProceduralPrecondition(GameObject agent)
    {
        target = GameObject.FindGameObjectWithTag("Player");
        if (target && Vector3.Distance(transform.position, target.transform.position) <= 5f)
        {
            AddEffect("hitPlayer", true);
            return true;
        }
        return false;
    }

    public override bool IsDone()
    {
        return hit;
    }

    public override bool Perform(GameObject agent)
    {
        //OnAttack();
        //OnDamaged();
        m_EnemyController.TryAtack(target.transform.position);
        Debug.Log("Pif");
        hit = true;
        
        return true;

    }

    public override bool RequiresInRange()
    {
        return true;
    }

    public override void Reset()
    {
        hit = false;
        target = null;
    }


    void OnAttack()
    {
        Animator.SetTrigger(k_AnimAttackParameter);
    }

    void OnDamaged()
    {
        if (RandomHitSparks.Length > 0)
        {
            int n = Random.Range(0, RandomHitSparks.Length - 1);
            RandomHitSparks[n].Play();
        }

        Animator.SetTrigger(k_AnimOnDamagedParameter);
    }

}
