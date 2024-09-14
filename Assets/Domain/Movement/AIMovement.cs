using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIMovement : MonoBehaviour, IMovement
{
    private NavMeshAgent navMeshAgent;
    private Vector3 currentDestination;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    public void SetDestination(Vector3 destination)
    {
        currentDestination = destination;
    }

    public void Move()
    {
        navMeshAgent.destination = currentDestination;
        // TODO: Add in a sanity check to return to home if in the presence of too much light
    }
}
