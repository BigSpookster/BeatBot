
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 0.125f;
    public Vector3 offset = new Vector3(0f, 0f, -10f);

    void LateUpdate()
    {
        if (player == null) return;

        // Follow the player on both X and Y axes
        Vector3 targetPosition = new Vector3(
            player.position.x + offset.x,
            player.position.y + offset.y,
            player.position.z + offset.z
        );

        // Smooth camera movement
        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            smoothSpeed
        );
    }
}
