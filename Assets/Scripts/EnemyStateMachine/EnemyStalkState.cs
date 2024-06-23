using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator), typeof(UnityEngine.AI.NavMeshAgent))]

public class EnemyStalkState : EnemyBaseState
{


    private GameObject targetMarker; // hold current marker object

    private float distanceToTarget; // holds distance to target every frame
    private Vector3 newTargetDestination; // holds next predicted space for target
    private float timeToTarget; // time left to reach target location

    private Vector3 targetVelocity; // holds the target's current velocity vector

    private float captureDistance; // buffer for successful capture of waypoint

    private bool blocked; // raycast bool for the destination target marker. If it hits omething, is true
    private UnityEngine.AI.NavMeshHit hit; // for raycast


    public override void EnterState(EnemyAIStateController enemy, Animator anim, NavMeshAgent agent)
    {
        Debug.Log("Hello from stalking state!");

        // set buffer
        captureDistance = 2;

        setNextWaypoint(enemy, agent);

        // ensure attack anim layer weight is 0 to avoid blending.
        anim.SetLayerWeight(2, 0.5f);
    }

    public override void UpdateState(EnemyAIStateController enemy, Animator anim, NavMeshAgent agent)
    {

        
        // if player seen, draw closer

            setNextWaypoint(enemy, agent); //  call waypoint iterator

            blocked = NavMesh.Raycast(agent.transform.position, newTargetDestination, out hit, NavMesh.AllAreas);

            // draw line for raycast
            Debug.DrawLine(agent.transform.position, newTargetDestination, blocked ? Color.red : Color.green);

            if (blocked)
                Debug.DrawRay(hit.position, Vector3.up, Color.red);


            if (targetMarker != null)
            {
                Object.Destroy(targetMarker);
            }
            // draw the target marker at predicted destination.
            targetMarker = Object.Instantiate(enemy.targetMarkerPrefab,
                newTargetDestination, Quaternion.identity);

            // set agent to move using the Mecanim animations
            anim.SetFloat("VelocityZ", agent.velocity.magnitude / agent.speed);



            if (agent.remainingDistance <= captureDistance)
            {
                Debug.Log("Target close!");

                if (targetMarker != null)
                {
                    Object.Destroy(targetMarker);
                }
                // agent stops moving!
                agent.isStopped = true;
                agent.speed = 0;

                // when close to Player, switch to attack state!
                enemy.ChangeState(enemy.enemyAttack);
            }
            else if (agent.remainingDistance > enemy.DetectionRadius)
            {
                // check if target still in FoV
                bool isPlayer = enemy.handleDetection(anim);
                if (!isPlayer)
                {
                    // go back to patrolling
                    enemy.ChangeState(enemy.enemyPatrol);
                }
            }
      
        

    }

    private void predictTargetPosition(EnemyAIStateController enemy, UnityEngine.AI.NavMeshAgent agent)
    {

        // distance to target = target pos - agent pos
        distanceToTarget = Vector3.Distance(enemy.target.transform.position, agent.transform.position);

        // get the time left till target is reached
        timeToTarget = Mathf.Clamp(distanceToTarget / agent.speed, 0.5f, 1);


        // where will target have moved in this time?
        newTargetDestination = enemy.target.transform.position + (timeToTarget * targetVelocity);

    }


    private void setNextWaypoint(EnemyAIStateController enemy, UnityEngine.AI.NavMeshAgent agent)
    {


        targetVelocity = enemy.target.GetComponent<VelocityReporter>().velocity;

        // get and set new destination
        predictTargetPosition(enemy, agent);

        agent.SetDestination(newTargetDestination);

    }
}
