using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{

    private float timer;

    public override void EnterState(EnemyAIStateController enemy, Animator anim, UnityEngine.AI.NavMeshAgent agent)
    {
        Debug.Log("Hello from attack state!");
        agent.isStopped = false;
        agent.speed = 2; // reset speed to default
        timer = 1f; // added a variable for placeholder delay (to allow attack animt o play out)

    }

    public override void UpdateState(EnemyAIStateController enemy, Animator anim, UnityEngine.AI.NavMeshAgent agent)
    {

        // attack once then continue stalking player
        anim.SetTrigger("IsAttacking");
        anim.SetLayerWeight(2, 1f);

        timer -= Time.deltaTime;
        if(timer <= 0f) { 
            enemy.ChangeState(enemy.enemyStalk);
        }

    }
}
