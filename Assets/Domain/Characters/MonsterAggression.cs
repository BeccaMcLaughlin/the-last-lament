using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(AIMovement))]
public class MonsterAggression : MonoBehaviour
{
    public float aggressionTimeout = 3f;
    public float aggressionRange = 5f;
    public float fieldOfViewAngle = 45f;
    public LayerMask obstaclesLayer;
    public List<Prey> prey = new List<Prey>();
    public GameObject currentlyChasing = null;
    public float chasingDelay = 0.2f;

    private bool isChasing = false;
    private SphereCollider sphereCollider;
    private AIMovement movement;
    private float chasingDelayNextTick;

    void Start()
    {
        movement = GetComponent<AIMovement>();

        // Set up a sphere collider
        sphereCollider = gameObject.AddComponent<SphereCollider>();
        sphereCollider.isTrigger = true;
        sphereCollider.radius = aggressionRange;
    }

    void Update()
    {
        ChasePrey();
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
        if (leavingPrey != null && prey.Contains(leavingPrey)) {
            prey.Remove(leavingPrey);
            CalculateThingToChase();
        }
    }

    private void CalculateThingToChase()
    {
        // Calculate a weighted value based on the priority of the prey.
        // For the player this is based on remaining health and distance.
        // Other objects inheriting Prey could use a fixed value.
        Prey topPriorityPrey = prey.Count > 0 ? prey.OrderBy(p => p.priority).FirstOrDefault() : null;

        if (topPriorityPrey == null)
        {
            currentlyChasing = null;
            return;
        }

        currentlyChasing = topPriorityPrey.gameObject;
    }

    private void ChasePrey()
    {
        if (!currentlyChasing) return;
        // float distanceToPrey = Vector3.Distance(transform.position, currentlyChasing.transform.position);

        if (Time.time >= chasingDelayNextTick)
        {
            chasingDelayNextTick = Time.time + chasingDelay;
            movement.SetDestination(currentlyChasing.transform.position);
            movement.Move();
        }
    }
}
