using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolState : EnemyBaseState
{
    private float detectionRadius;
    private float minDetectionAngle;
    private float maxDetectionAngle;

    private int currWaypoint;

    public int CurrWaypoint
    {
        get { return currWaypoint; }
        private set { currWaypoint = value; }
    }

    public float DetectionRadius
    {
        get { return detectionRadius; }
        private set { detectionRadius = value; }
    }

    public float MinDetectionAngle
    {
        get { return minDetectionAngle; }
        private set
        {

            Debug.Assert(value >= -180, "Angle min out of bounds!");
            minDetectionAngle = value;
        }
    }

    public float MaxDetectionAngle
    {
        get { return maxDetectionAngle; }
        private set
        {
            Debug.Assert(value <= 180, "Angle max out of bounds!");
            maxDetectionAngle = value;
        }
    }


    public override void EnterState(EnemyAIStateController enemy, Animator anim, UnityEngine.AI.NavMeshAgent agent)
    {
        Debug.Log("Hello from patrol state!");

        // init varaibles used only in this state
        CurrWaypoint = -1; //  init with dummy value
        MinDetectionAngle = -50;
        MaxDetectionAngle = 50;
        DetectionRadius = 10f;

        Debug.Log("Length of array: " + enemy.waypoints.Length);
        setNextWaypoint(enemy, agent); //  call waypoint iterator

    }

    public override void UpdateState(EnemyAIStateController enemy, Animator anim, UnityEngine.AI.NavMeshAgent agent)
    {

        // patrol!
        handleDetection(enemy);

        // check if agent has reached current waypoint
        // if yes, call next waypoint
        // ensure there is no pending path for it to alteady take
        // otherwise it will keep rerouting away from the nearest point




        if (agent.remainingDistance == 0 && !agent.pathPending)
        {
            //Debug.Log("If distance is true");
            //Debug.Assert(agent.remainingDistance == 0, "Distance not 0");

            //if (currWaypoint == enemy.waypoints.Length - 1)
            //{
            //    Debug.Log("Changing state!");
            //    // something
            //}

            setNextWaypoint(enemy, agent); //  call waypoint iterator

        }
        Debug.Log((agent.velocity.magnitude / agent.speed) + " is curent velocityZ.");
        // set agent to move using the Mecanim animations
        anim.SetFloat("VelocityZ", agent.velocity.magnitude / agent.speed);

    }

    private void setNextWaypoint(EnemyAIStateController enemy, UnityEngine.AI.NavMeshAgent agent)
    {

        Debug.Log("Hello from set waypoint function!");

        // check waypoints list isn't empty
        Debug.Assert(enemy.waypoints.Length > 0, "No waypoints!");

        // check if currWaypoint is in array, if not, set to 0


        if (currWaypoint < 0)
        {
            Debug.Log("Reached min index value!");
            currWaypoint = 0;
            agent.SetDestination(enemy.waypoints[currWaypoint].transform.position);
            Debug.Log("Next destination index: " + currWaypoint);
        }
        else
        {
            currWaypoint++;

            if (currWaypoint > enemy.waypoints.Length - 1)
            {
                Debug.Log("Reached max index value!");
                currWaypoint = 0;

            }
            agent.SetDestination(enemy.waypoints[currWaypoint].transform.position);
            Debug.Log("Next destination index: " + currWaypoint);

        }


    }

    public void handleDetection(EnemyAIStateController enemy)
    {
        // handle the Field of View of the enemy
        // if player is within their viewing angle, switch states

        // set up collision detection
        Collider[] colliders = Physics.OverlapSphere(enemy.transform.position, detectionRadius, enemy.detectionLayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            Debug.Log(colliders[i].name + " was hit!");

            // if player layer then check if it is in FoV
            if (colliders[i].gameObject.layer == 10)
            {

                // vector for the gap between enemy and player
                Vector3 targetDetection = colliders[i].transform.position - enemy.transform.position;
                // angle between where enemy is facing and where the target is
                float viewAngle = Vector3.Angle(enemy.transform.forward, targetDetection);
                // is it in FoV?
                if (viewAngle >= minDetectionAngle && viewAngle <= maxDetectionAngle)
                {

                    // switch to stalking state!
                    enemy.ChangeState(enemy.enemyStalk);
                }

            }
        }

    }
}
