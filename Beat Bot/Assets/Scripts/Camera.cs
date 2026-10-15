using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 0.125f;
    public Vector3 offset = new Vector3(0f, 2f, -10f);

    //This is used to determien when the camera moves upwards.
    public float yMovement = 3f;
    private float fixedY;

    void Start()
    {
        // Lock the initial Y position of the camera
        fixedY = transform.position.y;
    }

    void LateUpdate()
    {
        if (player == null) return;
        float targetY = fixedY;
        if(player.position.y > fixedY + yMovement)
        {
            targetY = player.position.y - yMovement + fixedY + offset.y;
        }
        else
        {
            targetY = fixedY + offset.y;
        }
        // Follow player's X, keep Y fixed, and maintain Z offset
        Vector3 targetPosition = new Vector3(player.position.x + offset.x, targetY, offset.z);
        
        // Smoothly interpolate position
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
    }
}