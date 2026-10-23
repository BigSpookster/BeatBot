using UnityEngine;

public class GameSession : MonoBehaviour
{   
    // Variable that other scripts can read, but only GameSession can change
    public static GameSession Instance {get; private set; }

    // Variables Fields
    [SerializeField] private string currentCheckpointID;
    [SerializeField] private string currentCheckpointScene;

    // Variables to allow other scripts to read checkpoint data
    public string CurrentCheckpointID => currentCheckpointID;
    public string CurrentCheckpointScene => currentCheckpointScene;

    // Variables to track specific info
    private int healthUpgradesCollected;   // How many upgrades the player has collected
    private float currentPlayerHealth;     // How much health the player currently has
    private bool hasPlayerHealth;          // Tracks if the player still have health

    // Return # of Upgrades Collected when called
    public int HealthUpgradesCollected => healthUpgradesCollected;

    // Return how much health does the player have?
    public float CurrentPlayerHealth => currentPlayerHealth;

    // Return if the player still has health
    public bool HasPlayerHealth => hasPlayerHealth;
    
    private void Awake()
    {
        // Check if a GameSession already exists (don't want duplicate sessions)
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // If it's the first session, keep it
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Set checkpoint to new checkpoint ID
    public void SetCheckpoint(string sceneName, string checkpointID)
    {
        currentCheckpointScene = sceneName;
        currentCheckpointID = checkpointID;
    }

    // Set the player health in the new scene
    public void SetPlayerHealth(float health)
    {
        currentPlayerHealth = health;
        hasPlayerHealth = true;
    }
}
