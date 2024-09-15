using UnityEngine;

public class Prey : MonoBehaviour
{
    public float priority = 1f;

    private IMovement movement;

    private void Start()
    {
        movement = GetComponent<IMovement>();
    }

    public bool isMakingNoise
    {
        get
        {
            if (movement is PlayerMovement playerMovement)
            {
                return !playerMovement.isCrouching();
            }

            return true;
        }
    }
}
