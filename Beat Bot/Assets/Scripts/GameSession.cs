using UnityEngine;

public class GameSession : MonoBehaviour, IDataPersistence
{   
    // Variable that other scripts can read, but only GameSession can change
    public static GameSession Instance {get; private set; }

    // Variables Fields
    [SerializeField] private string currentCheckpointID;
    [SerializeField] private string currentCheckpointScene;
    
    // Variables to track specific info
    [SerializeField] private int healthUpgradesCollected;   // How many upgrades the player has collected
    private float currentPlayerHealth;     // How much health the player currently has
    private bool hasPlayerHealth;          // Tracks if the player still have health
    private string nextSpawnPointID;       // Tracks where player should appear in next room/scene
    private string nextSpawnScene;         // Tracks which scene player should appear in next
    
    // Variables to allow other scripts to read checkpoint data
    public string CurrentCheckpointID => currentCheckpointID;
    public string CurrentCheckpointScene => currentCheckpointScene;
    public string NextSpawnPointID => nextSpawnPointID;
    public string NextSpawnScene => nextSpawnScene;

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

    // Set entry point where player should appear in a scene
    public void SetNextSpawnPoint(string sceneName, string spawnPointID)
    {
        nextSpawnScene = sceneName;
        nextSpawnPointID = spawnPointID;

        Debug.Log(
        "Next scene: " + nextSpawnScene +
        " | Spawn ID: " + nextSpawnPointID
        );
    }

    // Clear entry point
    public void ClearNextSpawnPoint()
    {
        nextSpawnScene = "";
        nextSpawnPointID = "";
    }

    //
    public void LoadData(GameData data)
    {
        healthUpgradesCollected = data.healthUpgradesCollected;
    }

    public void SaveData(ref GameData data)
    {
        data.healthUpgradesCollected = healthUpgradesCollected;
    }
}
