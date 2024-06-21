using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(Animator), typeof(UnityEngine.AI.NavMeshAgent))]

// manages all states for Minion
public class EnemyAIStateController : MonoBehaviour
{
    Animator anim; // hold animator component
    NavMeshAgent agent; // hold navmeshAI

    // for static waypoints
    public GameObject[] waypoints;

    // for moving waypoint
    public GameObject target; // hold target waypoint
    public GameObject targetMarkerPrefab; // hold prefab of predicted destination market

    // init each concrete state
    public EnemyPatrolState enemyPatrol = new EnemyPatrolState();
    public EnemyStalkState enemyStalk = new EnemyStalkState();
    public EnemyAttackState enemyAttack = new EnemyAttackState();
    public EnemyDeathState enemyDeath = new EnemyDeathState();

    // set current state
    EnemyBaseState currentState;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        currentState = enemyPatrol; //  set first state
        currentState.EnterState(this, anim, agent);
    }

    void Update()
    {
        currentState.UpdateState(this, anim, agent);
    }



    public void ChangeState(EnemyBaseState nextState)
    {
        // set current state to the new state to be started
        currentState = nextState;
        // move to the Enter stage of new state
        currentState.EnterState(this, anim, agent);
    }

}