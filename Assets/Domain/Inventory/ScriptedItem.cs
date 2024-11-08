using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Create New Ritual Item", order = 1)]
public class ScriptedItem : ScriptableObject
{
    public string ItemName = "New Item";
    public Sprite ItemIcon = null;
    public Rigidbody ItemSpawnObject = null;
    public Rigidbody MatchingRitualObject = null;
    public float PointCost = 1;
    public AudioClip AudioWhenUsed = null;
    public GameObject AvailableSpawnPoints = null;
}
