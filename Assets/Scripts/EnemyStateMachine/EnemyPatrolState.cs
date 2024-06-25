using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolState : EnemyBaseState
{
    
    

    private int currWaypoint;

    public int CurrWaypoint
    {
        get { return currWaypoint; }
        private set { currWaypoint = value; }
    }

    

    public override void EnterState(EnemyAIStateController enemy, Animator anim, UnityEngine.AI.NavMeshAgent agent)
    {
        Debug.Log("Hello from patrol state!");

        // init varaibles used only in this state
        CurrWaypoint = -1; //  init with dummy value
        agent.isStopped = false;
        agent.speed = 2; // default move speed

        //Debug.Log("Length of array: " + enemy.waypoints.Length);
        setNextWaypoint(enemy, anim, agent); //  call waypoint iterator

        // not attacking until close enough
        anim.ResetTrigger("IsAttacking");
        // ensure attack anim layer weight is 0 to avoid blending.
        anim.SetLayerWeight(2,0.0f);
    }

    public override void UpdateState(EnemyAIStateController enemy, Animator anim, UnityEngine.AI.NavMeshAgent agent)
    {

        // check if agent has reached current waypoint
        // if yes, call next waypoint
        // ensure there is no pending path for it to alteady take
        // otherwise it will keep rerouting away from the nearest point

        if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            setNextWaypoint(enemy, anim, agent); //  call waypoint iterator
        }
            //Debug.Log((agent.velocity.magnitude / agent.speed) + " is curent velocityZ.");
            // set agent to move using the Mecanim animations
        anim.SetFloat("VelocityZ", agent.velocity.magnitude / agent.speed);

        // verify the player isn't in detection zone
        // if it is, move to stalking state

        bool isPlayer = enemy.handleDetection();

        if (isPlayer)
        {
            enemy.ChangeState(enemy.enemyStalk);

        }
    }

    private void setNextWaypoint(EnemyAIStateController enemy, Animator anim, UnityEngine.AI.NavMeshAgent agent)
    {
        // generates next destination for the enemy
        // iterates through waypoints

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
