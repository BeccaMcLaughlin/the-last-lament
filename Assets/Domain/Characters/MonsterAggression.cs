using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        sphereCollider.center = Vector3.zero;
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
        if (newPrey != null && !prey.Contains(newPrey))
        {
            prey.Add(newPrey);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Prey leavingPrey = other.GetComponent<Prey>();
        if (leavingPrey != null && prey.Contains(leavingPrey))
        {
            prey.Remove(leavingPrey);
        }
    }

    private void CalculateThingToChase()
    {
        // Check if there are any valid prey in the list
        if (prey.Count == 0)
        {
            currentlyChasing = null;
            return;
        }

        // Find the highest priority prey that is either making noise or visible
        Prey topPriorityPrey = prey
            .Where(p => p.isMakingNoise || HasLineOfSight(p.gameObject))
            .OrderBy(p => p.priority)
            .FirstOrDefault();
        Debug.Log(topPriorityPrey);

        if (topPriorityPrey == null)
        {
            currentlyChasing = null;
            return;
        }

        // Set the currently chasing prey
        currentlyChasing = topPriorityPrey.gameObject;
    }

    private void ChasePrey()
    {
        // Ensure currentlyChasing is updated correctly before attempting to move
        if (Time.time >= chasingDelayNextTick)
        {
            CalculateThingToChase();

            // If no target is available, stop chasing
            if (!currentlyChasing) return;

            chasingDelayNextTick = Time.time + chasingDelay;

            // Set the destination and move if a valid prey is set
            movement.SetDestination(currentlyChasing.transform.position);
            movement.Move();
            GazeFollowsTarget();
        }
    }

    bool HasLineOfSight(GameObject prey)
    {
        Vector3 directionToPrey = (prey.transform.position - transform.position).normalized;
        if (Vector3.Angle(transform.forward, directionToPrey) < fieldOfViewAngle / 2)
        {
            if (Physics.Raycast(transform.position, directionToPrey, out RaycastHit hit, aggressionRange, obstaclesLayer))
            {
                // Validate that the object hit is indeed the prey and not something else
                if (hit.collider.gameObject == prey)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void GazeFollowsTarget()
    {
        if (!currentlyChasing) return;
        Vector3 direction = currentlyChasing.transform.position - transform.position;
        direction.y = 0;

        if (direction.magnitude > 0.1f)
        {
            Quaternion lookDirection = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookDirection, Time.deltaTime * movement.GetRotationSpeed());
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, aggressionRange);
    }
}
