using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(AIMovement))]
public class MonsterAggression : MonoBehaviour
{
    public float aggressionTimeout = 3f;
    public float aggressionRange = 5f;
    public float fieldOfViewAngle = 90f;
    public Animator animator;

    private SphereCollider sphereCollider;
    private AIMovement movement;
    private float chasingDelay = 0.2f;
    private float chasingDelayNextTick;
    private float resumePatrolTime;

    // Timer to track when the prey was last seen or heard
    private float lastSeenTime = 0f;
    private float noiseStopTime = 0f;
    private Prey currentlyChasingPrey;

    private LayerMask obstaclesLayer;

    void Start()
    {
        movement = GetComponent<AIMovement>();
        animator = GetComponent<Animator>();

        obstaclesLayer = LayerMask.GetMask("Obstacles");

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
        if (newPrey == null) return;

        // Check if the new prey has a higher priority than the current target
        if (currentlyChasingPrey == null || newPrey.priority < currentlyChasingPrey.priority)
        {
            // Evaluate the new prey's visibility and noise status
            EvaluatePrey(newPrey);
        }
    }

    private void CalculateThingToChase()
    {
        // Check if currently chasing prey exists
        if (currentlyChasingPrey == null)
        {
            ResumePatrolAfterDelay();
            return;
        }

        // Evaluate the current prey's visibility and noise status
        EvaluatePrey(currentlyChasingPrey);
    }

    private void EvaluatePrey(Prey prey)
    {
        bool hasLineOfSight = HasLineOfSight(prey.gameObject);

        // Update the last seen and noise times based on current states
        if (hasLineOfSight)
        {
            lastSeenTime = Time.time;
        }

        if (!prey.isMakingNoise)
        {
            noiseStopTime = Time.time;
        }

        // Stop chasing if line of sight is lost and noise stops within 0.5 seconds
        if (!hasLineOfSight && Time.time - noiseStopTime <= 0.5f)
        {
            currentlyChasingPrey = null;
            resumePatrolTime = Time.time + Random.Range(0.5f, 5f); // Set random time to resume patrol
        }
        else
        {
            currentlyChasingPrey = prey;
            movement.SetIsPatrolling(false); // Stop patrolling when chasing a new or current target
        }
    }

    private void ResumePatrolAfterDelay()
    {
        // Resume patrolling after the set delay time
        if (Time.time >= resumePatrolTime)
        {
            currentlyChasingPrey = null;
            movement.SetIsPatrolling(true);
        }
    }

    private void ChasePrey()
    {
        // Ensure currentlyChasingPrey is updated correctly before attempting to move
        if (Time.time >= chasingDelayNextTick)
        {
            chasingDelayNextTick = Time.time + chasingDelay;
            CalculateThingToChase();

            // If no target is available, stop chasing
            if (currentlyChasingPrey == null) return;

            // Set the destination and move if a valid prey is set
            movement.SetDestination(currentlyChasingPrey.transform.position);
            movement.Move();
            GazeFollowsTarget();
        }
    }

    bool HasLineOfSight(GameObject prey)
    {
        Vector3 directionToPrey = (prey.transform.position - transform.position).normalized;
        if (Vector3.Angle(transform.forward, directionToPrey) < fieldOfViewAngle / 2)
        {
            // Adjust the LayerMask to properly detect the prey and obstacles
            if (Physics.Raycast(transform.position, directionToPrey, out RaycastHit hit, aggressionRange, ~obstaclesLayer))
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
        if (currentlyChasingPrey == null) return;
        Vector3 direction = currentlyChasingPrey.transform.position - transform.position;
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

        // Draw forward facing line to visualize the monster's rotation
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * aggressionRange);

        // Draw field of view lines
        Vector3 leftBoundary = Quaternion.Euler(0, -fieldOfViewAngle / 2, 0) * transform.forward;
        Vector3 rightBoundary = Quaternion.Euler(0, fieldOfViewAngle / 2, 0) * transform.forward;

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary * aggressionRange);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary * aggressionRange);
    }
}
