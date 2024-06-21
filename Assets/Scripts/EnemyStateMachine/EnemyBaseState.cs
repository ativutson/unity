using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator), typeof(UnityEngine.AI.NavMeshAgent))]


public abstract class EnemyBaseState
{
    // init the states the Minion goes through
    public abstract void EnterState(EnemyAIStateController enemy, Animator anim, NavMeshAgent agent);

    public abstract void UpdateState(EnemyAIStateController enemy, Animator anim, NavMeshAgent agent);

}

