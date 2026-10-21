using UnityEngine;

public class GameSession : MonoBehaviour
{
    private static GameSession Instance;

    [SerializeField] private float baseHealth;
    [SerializeField] private float healthUpgradesCollected;
    [SerializeField] private string currentCheckpointID;
    [SerializeField] private string currentCheckpointScene;

    private int maxHealth;
    public string CurrentCheckpointID => currentCheckpointID;
    public string CurrentCheckpointScene => currentCheckpointScene;


    private void Awake()
    {
        // If we already have an instance, don't create another one
        if(Instance != null && Instance != .this)
        {
            destroy(GameObject);
            return;
        }
    }

    public void setCheckpoint(string sceneName, string checkpointID)
    {
        currentCheckpointScene = sceneName;
        currentCheckpointID = checkpointID;
    }

    public float CalculateHealth()
    {
        maxHealth = baseHealth + GameSession.Instance.healthUpgradesCollected;
        return maxHealth;
    }
    // Keeps all values between scene reloads
    DontDestroyOnLoad(gameObject);
}
