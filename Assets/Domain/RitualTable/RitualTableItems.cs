using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class RitualTableItems : MonoBehaviour
{
    public List<ScriptedItem> itemPool = new List<ScriptedItem>();
    public List<Transform> ritualItemSpawnPoints = new List<Transform>();

    private List<ScriptedItem> currentItemPool = new List<ScriptedItem>();
    private int maximumItemsOnTable = 6;

    public static event Action CompleteRitual;

    private void OnEnable()
    {
        RitualMatch.OnHandleMatch += HandleItemMatchedAtLocation;
    }

    private void OnDisable()
    {
        RitualMatch.OnHandleMatch -= HandleItemMatchedAtLocation;
    }

    public void SelectItemsBasedOnCorruption()
    {
        float availableCorruption = GameState.Corruption;
        float maxPointCostOfSingleItem = Mathf.Ceil(GameState.Corruption / UnityEngine.Random.Range(3, 5));
        Debug.Log(maxPointCostOfSingleItem);
        float attemptsToSelectItem = 1;
        List<ScriptedItem> affordableItems = itemPool.FindAll(item => item.PointCost <= maxPointCostOfSingleItem);

        if (affordableItems.Count == 0)
        {
            Debug.LogError("No afforable items in the pool");
            return;
        }

        while (availableCorruption > 0 && currentItemPool.Count < maximumItemsOnTable)
        {
            attemptsToSelectItem += 1;
            ScriptedItem selectedItem = affordableItems[UnityEngine.Random.Range(0, affordableItems.Count - 1)];

            if (selectedItem.PointCost > availableCorruption)
            {
                continue;
            }

            currentItemPool.Add(selectedItem);
            SpawnItem(selectedItem, currentItemPool.Count - 1);
            availableCorruption -= selectedItem.PointCost;

            if (currentItemPool.Count >= maximumItemsOnTable || attemptsToSelectItem > 20)
            {
                break;
            }
        }

        SpawnMatchingRitualItems();
    }

    private void SpawnItem(ScriptedItem item, int index)
    {
        // Check if the item prefab is not null
        if (item.AvailableSpawnPoints == null)
        {
            Debug.LogError($"No spawn prefab assigned for item: {item.ItemName}");
            return;
        }

        GameObject spawnContainer = Instantiate(item.AvailableSpawnPoints);
        Transform spawnParent = spawnContainer.transform.Find("SpawnPoint");
        List<Transform> spawnPoints = new List<Transform>(spawnParent.GetComponentsInChildren<Transform>());

        if (spawnPoints.Count == 0)
        {
            Debug.LogError("No available spawn points found within the item prefab.");
            Destroy(spawnContainer); // Clean up the unused container
            return;
        }

        // TODO: Technically, two items could spawn at the same point which we don't want.
        Transform selectedSpawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Count)];
        GameObject spawnedItem = Instantiate(item.ItemSpawnObject.gameObject, selectedSpawnPoint.position, selectedSpawnPoint.rotation);

        Destroy(spawnContainer);
    }

    private void SpawnMatchingRitualItems()
    {
        for (int i = 0; i < currentItemPool.Count; i++)
        {
            GameObject spawnedItem = Instantiate(currentItemPool[i].MatchingRitualObject.gameObject, ritualItemSpawnPoints[i].position, ritualItemSpawnPoints[i].rotation);

            // Update ritual index of this item
            spawnedItem.GetComponent<RitualMatch>().ritualIndex = i;
        }
    }

    // Listen for event that passes the transformation through and remove from the item pool where the transform matches
    private void HandleItemMatchedAtLocation(int index)
    {
        Debug.Log("Handle ritual match");

        // Update to remove at index
        currentItemPool.RemoveAt(index);

        if (currentItemPool.Count == 0 && GameState.HasCorpseOnTable)
        {
            CompleteRitual?.Invoke();
        }
    }
}
