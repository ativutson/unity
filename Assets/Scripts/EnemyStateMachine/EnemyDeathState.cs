using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathState : EnemyBaseState
{

    Ragdoll ragdoll;
    private float timeToFade;

    public override void EnterState(EnemyAIStateController enemy, Animator anim, UnityEngine.AI.NavMeshAgent agent)
    {
        Debug.Log("Hello from death state!");

        ragdoll = enemy.GetComponent<Ragdoll>();
        if (ragdoll == null)
        {
            Debug.Log("No Health component!");
        }

        ragdoll.ActivateRagdoll();
        timeToFade = 3f;

    }

    public override void UpdateState(EnemyAIStateController enemy, Animator anim, UnityEngine.AI.NavMeshAgent agent)
    {

        

        timeToFade -= Time.deltaTime;
        if(timeToFade == 0)
        {
            enemy.gameObject.SetActive(false);
        }

    }
}
