using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class MonsterAggression : MonoBehaviour
{
    public float aggressionTimeout = 3f;
    public float aggressionRange = 5f;
    public float fieldOfViewAngle = 45f;
    public LayerMask obstaclesLayer;
    public List<Prey> prey = new List<Prey>();
    public Prey currentlyChasing = null;

    private NavMeshAgent navMeshAgent;
    private bool isChasing = false;
    private SphereCollider sphereCollider;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        // Set up a sphere collider
        sphereCollider = gameObject.AddComponent<SphereCollider>();
        sphereCollider.isTrigger = true;
        sphereCollider.radius = aggressionRange;
    }

    void Update()
    {
       
    }

    private void OnTriggerEnter(Collider other)
    {
        Prey newPrey = other.GetComponent<Prey>();
        if (newPrey != null && !prey.Contains(newPrey)) {
            prey.Add(newPrey);
            CalculateThingToChase();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Prey leavingPrey = other.GetComponent<Prey>();
        if (prey != null && prey.Contains(leavingPrey)) {
            prey.Remove(leavingPrey);
            CalculateThingToChase();
        }
    }

    private void CalculateThingToChase()
    {
        // Calculate a weighted value based on the priority of the prey.
        // For the player this is based on remaining health and distance.
        // Other objects inheriting Prey could use a fixed value.
        currentlyChasing = prey.Count > 0 ? prey.OrderBy(p => p.priority).First() : null;
    }

    private void ChasePrey()
    {
        if (!currentlyChasing) return;
        float distanceToPrey = Vector3.Distance(transform.position, currentlyChasing.position);

        // Do navmesh and stuff
    }
}
