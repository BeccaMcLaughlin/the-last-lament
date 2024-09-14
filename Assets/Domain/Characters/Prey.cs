using UnityEngine;

abstract class Prey : MonoBehaviour
{
    public float priority = 1f;

    public bool isCrouching()
    {
        return false;
    }

    public bool isMakingNoise()
    {
        return false;
    }
}
