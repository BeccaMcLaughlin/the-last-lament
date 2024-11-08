using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(FPSController))]
public class PlayerMovement : MonoBehaviour, IMovement
{
    public float baseSpeed = 3f;

    private CharacterController controller;
    private FPSController playerController;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerController = GetComponent<FPSController>();
    }

    public float CurrentSpeed
    {
        get
        {
            float speed = baseSpeed;

            if (GameState.PlayerIsDraggingCorpse)
            {
                speed /= 0.5f;
            }
            else if (isSprinting())
            {
                speed *= 2f;
            }
            else if (isCrouching())
            {
                speed *= 0.5f;
            }

            return speed;
        }
    }

    public void Move()
    {
        Vector2 moveValue = PlayerActions.Instance.MoveValue;
        float x = moveValue.x;
        float z = moveValue.y;

        Vector3 movement = transform.right * x + transform.forward * z;
        Vector3 direction = new Vector3(CurrentSpeed * movement.x, -2f, CurrentSpeed * movement.z);
        controller.Move(direction * Time.deltaTime);
    }

    public bool isSprinting()
    {
        return PlayerActions.Instance.IsSprinting && playerController.currentStamina > 0f;
    }

    public bool isCrouching()
    {
        return PlayerActions.Instance.IsCrouching;
    }
}
