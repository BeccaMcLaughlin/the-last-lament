using UnityEngine;

public class MonsterAggression : MonoBehaviour
{
    public float aggressionTimeout = 3f;
    public float aggressionRange = 5f;
    public float fieldOfViewAngle = 45f;
    public LayerMask obstaclesLayer;
    public List<Prey> prey = new List<Prey>();
    public Transform currentlyChasing = null;

    private NavMeshAgent navMeshAgent;
    private boolean isChasing = false;
    private SphereCollider sphereCollider;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        // Set up a sphere collider
        sphereCollider = GameObject.AddComponent<SphereCollider>();
        sphereCollider.isTrigger = true;
        sphereCollider.radius = aggressionRange;
    }

    void Update()
    {
        if (prey.)
        {
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Prey prey = other.GetComponent<Prey>();
        if (prey != null && !prey.Contains(prey)) {
            prey.Add(prey);
            CalculateThingToChase();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Prey prey = other.GetComponent<Prey>();
        if (prey != null && prey.Contains(prey)) {
            prey.Remove(prey);
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
