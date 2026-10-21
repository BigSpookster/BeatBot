using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour
{
    // Fields
    private SpriteRenderer playerSprite;
    private PlayerMovement playerMovement;
    private Rigidbody2D rb;
    private Collider2D playerCollider;

    // Gets current values of player components
    private void Awake()
    {
        // Grab current component values of player
        playerMovement = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
        playerSprite = GetComponentInChildren<SpriteRenderer>();
        playerCollider = GetComponent<Collider2D>();
    }

    // When the player dies, make sure a GameSession exists and find the checkpoint
    private void Start()
    {
        // Make sure a GameSession exists
        if(GameSession.Instance == null)
        {
            return;
        }

        // Make sure the saved checkpoint belongs to the scene
        if(GameSession.Instance.CurrentCheckpointScene != SceneManager.GetActiveScene().name)
        {
            return;
        }

        // Find the correct checkpoint by saved ID
        Checkpoint[] checkpoints = FindObjectsByType<Checkpoint>(FindObjectsSortMode.None);

        // Find the checkpoint whose ID matches the saved ID
        foreach(Checkpoint checkpoint in checkpoints)
        {
            // Make the checkpoint ID match the saved ID
            if(checkpoint.CheckpointID == GameSession.Instance.CurrentCheckpointID)
            {
                transform.position = checkpoint.SpawnPosition;
                break;
            }
        }
    } 
    // Called when player dies
    public void HandleDeath()
    {
        // Prevent player from moving/interacting with anything after death
        rb.linearVelocity = Vector2.zero;
        playerMovement.enabled = false;
        playerSprite.enabled = false;
        playerCollider.enabled = false;
        rb.simulated = false;
    }

    // Reload the scene at the respawn point
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
