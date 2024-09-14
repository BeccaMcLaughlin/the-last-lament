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
    public List<Prey> prey = new List<Prey>();
    public GameObject currentlyChasing = null;
    public Animator animator;
    public LayerMask obstaclesLayer;

    private SphereCollider sphereCollider;
    private AIMovement movement;
    private float chasingDelay = 0.2f;
    private float chasingDelayNextTick;

    void Start()
    {
        movement = GetComponent<AIMovement>();
        animator = GetComponent<Animator>();

        // Set up a sphere collider
        sphereCollider = gameObject.AddComponent<SphereCollider>();
        sphereCollider.isTrigger = true;
        sphereCollider.radius = aggressionRange;
    }

    void Update()
    {
        ChasePrey();

        if (animator != null)
        {
            animator.SetFloat("speed", movement.velocity);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Prey newPrey = other.GetComponent<Prey>();
        if (newPrey != null && !prey.Contains(newPrey)) {
            prey.Add(newPrey);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Prey leavingPrey = other.GetComponent<Prey>();
        if (leavingPrey != null && prey.Contains(leavingPrey)) {
            prey.Remove(leavingPrey);
        }
    }

    private void CalculateThingToChase()
    {
        // Calculate a weighted value based on the priority of the prey.
        // For the player this is based on remaining health and distance.
        // Other objects inheriting Prey could use a fixed value.
        Prey topPriorityPrey = prey.Count > 0 ? prey
            .Where(p =>
            {
                Debug.Log(p.isMakingNoise);
            return p.isMakingNoise || HasLineOfSight(p.gameObject);
    })
            .OrderBy(p => p.priority)
            .FirstOrDefault() : null;

        // TODO: Add animation where the monster stops and looks towards the new source of sound if topPriorityPrey.gameObject is not equal 
        // To currentlyChasing.gameObject

        if (topPriorityPrey == null)
        {
            currentlyChasing = null;
            return;
        }

        currentlyChasing = topPriorityPrey.gameObject;
    }

    private void ChasePrey()
    {
        // float distanceToPrey = Vector3.Distance(transform.position, currentlyChasing.transform.position);

        if (Time.time >= chasingDelayNextTick)
        {
            CalculateThingToChase();

            if (!currentlyChasing) return;

            chasingDelayNextTick = Time.time + chasingDelay;
            movement.SetDestination(currentlyChasing.transform.position);
            movement.Move();
        }
    }

    bool HasLineOfSight(GameObject prey)
    {
        Vector3 directionToPrey = (prey.transform.position - transform.position).normalized;
        if (Vector3.Angle(transform.forward, directionToPrey) < fieldOfViewAngle / 2)
        {
            if (!Physics.Raycast(transform.position, directionToPrey, out RaycastHit hit, aggressionRange, obstaclesLayer))
            {
                return true;
            }
        }
        return false;
    }
}
