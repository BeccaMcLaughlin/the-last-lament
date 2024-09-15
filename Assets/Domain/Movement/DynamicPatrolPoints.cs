using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class DynamicPatrolPoints : MonoBehaviour
{
    public float patrolRadius = 20f; // Radius around the spawn point to search for patrol points
    private Vector3 spawnPosition;
    private List<Transform> patrolPoints = new List<Transform>();

    private NavMeshAgent navMeshAgent;

    void Awake()
    {
        spawnPosition = transform.position;
        navMeshAgent = GetComponent<NavMeshAgent>();
        PopulatePatrolPoints();
        OrderPatrolPointsByProximity();
    }

    private void PopulatePatrolPoints()
    {
        // Find all objects with the tag "PatrolPoint"
        GameObject[] foundPatrolPoints = GameObject.FindGameObjectsWithTag("PatrolPoints");

        // Filter the patrol points within the specified radius of the spawn position
        foreach (GameObject point in foundPatrolPoints)
        {
            float distance = Vector3.Distance(spawnPosition, point.transform.position);
            if (distance <= patrolRadius)
            {
                patrolPoints.Add(point.transform);
            }
        }
    }

    private void OrderPatrolPointsByProximity()
    {
        if (patrolPoints.Count < 2) return; // No need to order if there's only one or no patrol points

        // Start ordering from the first point in the list
        List<Transform> orderedPoints = new List<Transform>();
        Transform currentPoint = patrolPoints[0];
        orderedPoints.Add(currentPoint);
        patrolPoints.Remove(currentPoint);

        while (patrolPoints.Count > 0)
        {
            Transform closestPoint = FindClosestPoint(currentPoint, patrolPoints);
            orderedPoints.Add(closestPoint);
            patrolPoints.Remove(closestPoint);
            currentPoint = closestPoint;
        }

        patrolPoints = orderedPoints;
    }

    private Transform FindClosestPoint(Transform fromPoint, List<Transform> points)
    {
        Transform closestPoint = null;
        float shortestDistance = float.MaxValue;

        foreach (var point in points)
        {
            float pathDistance = CalculatePathDistance(fromPoint.position, point.position);
            if (pathDistance < shortestDistance)
            {
                shortestDistance = pathDistance;
                closestPoint = point;
            }
        }

        return closestPoint;
    }

    private float CalculatePathDistance(Vector3 start, Vector3 end)
    {
        NavMeshPath path = new NavMeshPath();
        navMeshAgent.CalculatePath(end, path);
        float pathLength = 0f;

        if (path.status == NavMeshPathStatus.PathComplete)
        {
            for (int i = 1; i < path.corners.Length; i++)
            {
                pathLength += Vector3.Distance(path.corners[i - 1], path.corners[i]);
            }
        }
        else
        {
            return float.MaxValue; // Path is not complete; avoid this point
        }

        return pathLength;
    }

    public List<Transform> GetPatrolPoints()
    {
        return patrolPoints;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, patrolRadius);
    }
}
