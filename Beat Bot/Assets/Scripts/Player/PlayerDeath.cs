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

        // SceneSpawnPont logic (For room-to-room traversal)
        // Check if player entered the scene from another scene
        if(GameSession.Instance.NextSpawnScene == SceneManager.GetActiveScene().name)
        {
            Debug.Log("Trying to spawn in scene: " + GameSession.Instance.NextSpawnScene + " | Looking for ID: " + GameSession.Instance.NextSpawnPointID);

            SceneSpawnPoint[] spawnPoints = FindObjectsByType<SceneSpawnPoint>(FindObjectsSortMode.None);

            Debug.Log("Number of SceneSpawnPoints found: " + spawnPoints.Length);

            foreach(SceneSpawnPoint spawnPoint in spawnPoints)
            {
                Debug.Log(
                "Found SceneSpawnPoint on object: " +
                spawnPoint.gameObject.name +
                " | ID: " +
                spawnPoint.SpawnPointID
                );

                if(spawnPoint.SpawnPointID == GameSession.Instance.NextSpawnPointID)
                {
                    Debug.Log(
                    "MATCH FOUND: " +
                    spawnPoint.SpawnPointID
                    );
                    transform.position = spawnPoint.transform.position;

                    GameSession.Instance.ClearNextSpawnPoint();

                    return;
                }
            }

            Debug.LogError(
            "No SceneSpawnPoint matched ID: " +
            GameSession.Instance.NextSpawnPointID
            );
            return;
        }

        // Checkpoint logic (Death/SavePoint Respawning)
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
        // If we have a saved checkpoint scene, return to it
        if(GameSession.Instance != null && !string.IsNullOrEmpty(GameSession.Instance.CurrentCheckpointScene))
        {
            SceneManager.LoadScene(GameSession.Instance.CurrentCheckpointScene);
        }
        // Otherwise, just reload the scene we died in
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
