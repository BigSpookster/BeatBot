using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private string checkpointID;
    [SerializeField] private Transform spawnPoint;

    // Allow PlayerDeath to read checkpoint information
    public string CheckpointID => checkpointID;
    public Vector3 SpawnPosition => spawnPoint.position;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            // Set this as the player's current checkpoint
            GameSession.Instance.SetCheckpoint(
                SceneManager.GetActiveScene().name,
                checkpointID
            );

            // Save the updated checkpoint to disk
            DataPersistenceManager.instance.SaveGame();

            Debug.Log(
                "Game saved at checkpoint: " +
                checkpointID +
                " | Scene: " +
                SceneManager.GetActiveScene().name
            );
        }
    }
}
