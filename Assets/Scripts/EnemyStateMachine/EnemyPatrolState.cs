using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolState : EnemyBaseState
{


    private int currWaypoint;

    public override void EnterState(EnemyAIStateController enemy, Animator anim, UnityEngine.AI.NavMeshAgent agent)
    {
        Debug.Log("Hello from patrol state!");
        currWaypoint = -1; //  init with dummy value

        Debug.Log("Length of array: " + enemy.waypoints.Length);
        setNextWaypoint(enemy, agent); //  call waypoint iterator

    }

    public override void UpdateState(EnemyAIStateController enemy, Animator anim, UnityEngine.AI.NavMeshAgent agent)
    {

        // patrol!

        // check if agent has reached current waypoint
        // if yes, call next waypoint
        // ensure there is no pending path for it to alteady take
        // otherwise it will keep rerouting away from the nearest point
        //Debug.Assert(currWaypoint< enemy.waypoints.Length,"Length out of bounds!");




        if (agent.remainingDistance == 0 && !agent.pathPending)
        {
            //Debug.Log("If distance is true");
            //Debug.Assert(agent.remainingDistance == 0, "Distance not 0");

            if (currWaypoint == enemy.waypoints.Length - 1)
            {
                Debug.Log("Changing state!");
                enemy.ChangeState(enemy.enemyStalk);
                //return;
            }

            setNextWaypoint(enemy, agent); //  call waypoint iterator

        }
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
}
