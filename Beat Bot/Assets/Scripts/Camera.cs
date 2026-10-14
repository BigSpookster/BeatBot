using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 0.125f;
    public Vector3 offset = new Vector3(0f, 2f, -10f);

    private float fixedY;

    void Start()
    {
        // Lock the initial Y position of the camera
        fixedY = transform.position.y;
    }

    void LateUpdate()
    {
        if (player == null) return;

        // Follow player's X, keep Y fixed, and maintain Z offset
        Vector3 targetPosition = new Vector3(player.position.x + offset.x, fixedY, offset.z);
        
        // Smoothly interpolate position
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
    }
}