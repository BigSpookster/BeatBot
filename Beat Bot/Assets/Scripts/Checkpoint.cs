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
            GameSession.Instance.SetCheckpoint(SceneManager.GetActiveScene().name, checkpointID);
        }
    }
}
