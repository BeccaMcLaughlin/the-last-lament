using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class RitualTableItems : MonoBehaviour
{
    public List<ScriptedItem> itemPool = new List<ScriptedItem>();
    public List<Transform> ritualItemSpawnPoints = new List<Transform>();

    private List<ScriptedItem> currentItemPool = new List<ScriptedItem>();
    private int maximumItemsOnTable = 6;

    public void SelectItemsBasedOnCorruption()
    {
        float availableCorruption = GameState.Corruption;
        float maxPointCostOfSingleItem = Mathf.Ceil(GameState.Corruption / Random.Range(3, 5));
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
            ScriptedItem selectedItem = affordableItems[Random.Range(0, affordableItems.Count - 1)];

            if (selectedItem.PointCost > availableCorruption)
            {
                continue;
            }

            currentItemPool.Add(selectedItem);
            SpawnItem(selectedItem);
            availableCorruption -= selectedItem.PointCost;

            if (currentItemPool.Count >= maximumItemsOnTable || attemptsToSelectItem > 20)
            {
                break;
            }
        }

        SpawnMatchingRitualItems();
    }

    private void SpawnItem(ScriptedItem item)
    {
        // Check if the item prefab is not null
        if (item.AvailableSpawnPoints == null)
        {
            Debug.LogError($"No spawn prefab assigned for item: {item.ItemName}");
            return;
        }

        GameObject spawnContainer = Instantiate(item.AvailableSpawnPoints);
        Transform spawnParent = spawnContainer.transform.Find("SpawnPoint");
        List<Transform> spawnPoints = new List<Transform>(spawnContainer.GetComponentsInChildren<Transform>());

        if (spawnPoints.Count == 0)
        {
            Debug.LogError("No available spawn points found within the item prefab.");
            Destroy(spawnContainer); // Clean up the unused container
            return;
        }

        // TODO: Technically, two items could spawn at the same point which we don't want.
        Transform selectedSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
        GameObject spawnedItem = Instantiate(item.ItemSpawnObject.gameObject, selectedSpawnPoint.position, selectedSpawnPoint.rotation);

        Destroy(spawnContainer);
    }

    private void SpawnMatchingRitualItems()
    {
        for (int i = 0; i < currentItemPool.Count; i++)
        {
            Debug.Log("Instantiate ritual item");
            Debug.Log(currentItemPool[i].ItemName);
            GameObject spawnedItem = Instantiate(currentItemPool[i].MatchingRitualObject.gameObject, ritualItemSpawnPoints[i].position, ritualItemSpawnPoints[i].rotation);
        }
    }
}
