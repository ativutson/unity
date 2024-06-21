using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{


    public override void EnterState(EnemyAIStateController enemy, Animator anim, UnityEngine.AI.NavMeshAgent agent)
    {
        Debug.Log("Hello from attack state!");

    }

    public override void UpdateState(EnemyAIStateController enemy, Animator anim, UnityEngine.AI.NavMeshAgent agent)
    {



    }
}
