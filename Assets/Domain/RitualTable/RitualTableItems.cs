using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class RitualTableItems : MonoBehaviour
{
    public List<ScriptedItem> itemPool = new List<ScriptedItem>();

    private List<Pickup> pickups = new List<Pickup>();
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

        while (availableCorruption > 0 && pickups.Count < maximumItemsOnTable)
        {
            attemptsToSelectItem += 1;
            ScriptedItem selectedItem = affordableItems[Random.Range(0, affordableItems.Count - 1)];

            if (selectedItem.PointCost > availableCorruption)
            {
                continue;
            }

            itemPool.Add(selectedItem);
            SpawnItem(selectedItem);
            availableCorruption -= selectedItem.PointCost;
            Debug.Log(selectedItem.ItemName);

            if (itemPool.Count >= 6 || attemptsToSelectItem > 20)
            {
                break;
            }
        }
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

        Transform selectedSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
        GameObject spawnedItem = Instantiate(item.ItemSpawnObject.gameObject, selectedSpawnPoint.position, selectedSpawnPoint.rotation);

        Destroy(spawnContainer);
    }

    public void ClearPickups()
    {

    }
}
