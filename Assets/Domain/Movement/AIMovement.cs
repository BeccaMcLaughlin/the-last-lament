using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(DynamicPatrolPoints))]
public class AIMovement : MonoBehaviour, IMovement
{
    private List<Transform> patrolPoints;
    private int currentPatrolIndex = 0;
    private bool isPatrolling = true;

    private NavMeshAgent navMeshAgent;
    private Vector3 currentDestination;

    private float movementDelay = 0.2f;
    private float movementDelayNextTick;

    private DynamicPatrolPoints patrolSystem;

    public float velocity
    {
        get
        {
            return navMeshAgent.desiredVelocity.sqrMagnitude;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;

        // Get the patrol system and retrieve ordered patrol points
        patrolSystem = GetComponent<DynamicPatrolPoints>();
        patrolPoints = patrolSystem.GetPatrolPoints();

        StartPatrol();
    }

    void Update()
    {
        // Don't update each frame
        if (Time.time >= movementDelayNextTick)
        {
            movementDelayNextTick = Time.time + movementDelay;

            if (isPatrolling && navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance + 0.5f)
            {
                GoToNextPatrolPoint();
            }
        }
    }

    public void SetDestination(Vector3 destination)
    {
        currentDestination = destination;
    }

    public float GetRotationSpeed()
    {
        return navMeshAgent.angularSpeed;
    }

    public void Move()
    {
        navMeshAgent.destination = currentDestination;
        // TODO: Add in a sanity check to return to home if in the presence of too much light
    }

    private void StartPatrol()
    {
        if (patrolPoints == null || patrolPoints.Count == 0) return;

        navMeshAgent.SetDestination(patrolPoints[currentPatrolIndex].position);
    }

    private void GoToNextPatrolPoint()
    {
        if (patrolPoints == null || patrolPoints.Count == 0) return;

        // Loop it if doesn't fit
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;
        Debug.Log(currentPatrolIndex);
        StartPatrol();
    }
}
