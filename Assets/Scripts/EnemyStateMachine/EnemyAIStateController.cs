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

    // for detection (enemy FoV)

    // determine what layer the enemies look for (player)
    public LayerMask detectionLayer; // may be obselete
    public LayerMask playerLayer; // may be obselete
    private AISensor sensor; // new method

    // set the enemy's view radius
    private float detectionRadius; // may be obselete
    //private float minDetectionAngle; // may be obselete
    //private float maxDetectionAngle; // may be obselete


    // death state detection
    private EnemyHealthScript health; // holds enemy's health script

    public float DetectionRadius
    {
        get { return detectionRadius; }
        private set { detectionRadius = value; }
    }

    //public float MinDetectionAngle
    //{
    //    get { return minDetectionAngle; }
    //    private set
    //    {

    //        Debug.Assert(value >= -180, "Angle min out of bounds!");
    //        minDetectionAngle = value;
    //    }
    //}

    //public float MaxDetectionAngle
    //{
    //    get { return maxDetectionAngle; }
    //    private set
    //    {
    //        Debug.Assert(value <= 180, "Angle max out of bounds!");
    //        maxDetectionAngle = value;
    //    }
    //}



    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        currentState = enemyPatrol; //  set first state
        currentState.EnterState(this, anim, agent);

        // script to handle player detection (v2)
        sensor = GetComponent<AISensor>();
        if (sensor == null)
        {
            Debug.Log("No AISensor component!");
        }
        // add a couple layers to our detection for enemy FoV (so it can't see through things)
        detectionLayer |= (1 << 0);

        DetectionRadius = sensor.distance; // how far can enemy see
        //MinDetectionAngle = -50;
        //MaxDetectionAngle = 50;


        health = GetComponent<EnemyHealthScript>();
        if(health == null)
        {
            Debug.Log("No Health component!");
        }

    }

    void Update()
    {
        // check for death;
        //Debug.Log("Check health");
        isKilled(anim,health);

        currentState.UpdateState(this, anim, agent);
    }



    public void ChangeState(EnemyBaseState nextState)
    {
        // set current state to the new state to be started
        currentState = nextState;
        // move to the Enter stage of new state
        currentState.EnterState(this, anim, agent);
    }

    // handle FoV detection by enemy
    // may be called across various states so we're putting it in here

    //AISensor

    public bool handleDetection() {

        // if player game object on player layer found
        if(sensor.Objects.Count>0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void isBloodied(Animator anim)
    {
        // @TODO
        // if enemy is below x health, toggle injured animation layer
        
    }

    public void isKilled(Animator anim, EnemyHealthScript health)
    {
        // if enemy is 0, toggle ragdoll
        // enter death state
        if (health.CurrentHealth == 0f) {

            // putting this here so that we can reuse ragdoll code for Player
            agent.speed = 0;
            agent.isStopped = true;
            agent.acceleration = 0;
            agent.enabled = false;
            ChangeState(enemyDeath);
        }
    }


    //public bool handleDetection(Animator anim)
    //{
    //    // handle the Field of View of the enemy
    //    // if player is within their viewing angle, switch states

    //    // set up collision detection
    //    Collider[] colliders = Physics.OverlapSphere(this.transform.position, detectionRadius, detectionLayer);

    //    for (int i = 0; i < colliders.Length; i++)
    //    {
    //        //Debug.Log(colliders[i].name + " was hit!");

    //        // if player layer then check if it is in FoV
    //        if (colliders[i].gameObject.layer == 10)
    //        {
    //            Debug.Log("Player found!");

    //            // do a raycast to see if the player is behind anything
    //            // get direction vector toward player
    //            Vector3 playerDirection = transform.position - colliders[i].gameObject.transform.position;
    //            bool isVisible = Physics.Raycast(transform.position, playerDirection, detectionRadius);

    //            if (isVisible)
    //            {

    //                // vector for the gap between enemy and player
    //                Vector3 targetDetection = colliders[0].transform.position - transform.position;
    //                // angle between where enemy is facing and where the target is
    //                float viewAngle = Vector3.Angle(transform.forward, targetDetection);
    //                // is it in FoV?
    //                if (viewAngle >= minDetectionAngle && viewAngle <= maxDetectionAngle)
    //                {

    //                    Debug.Log("Player is visible!");
    //                    return true;
    //                }
    //            }

    //        }
    //    }

    //    return false;

    //}

}