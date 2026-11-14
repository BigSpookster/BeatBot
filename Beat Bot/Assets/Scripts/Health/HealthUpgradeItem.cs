using UnityEngine;

public class HealthUpgradeItem : MonoBehaviour
{
    // Unique ID Variable
    [SerializeField] private string id;
    [ContextMenu("Generate guid for id")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }

    // Variable to track whetehr its been collected
    private bool collected = false;

    private SpriteRenderer visual;
    private Collider2D pickupCollider;

    private void Start()
    {
        if(GameSession.Instance != null)
        {
            collected =
                GameSession.Instance.HasCollectedHealthUpgrade(id);

            UpdateCollectedState();
        }
    }

    private void Awake()
    {
        visual = GetComponentInChildren<SpriteRenderer>();
        pickupCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // If it's been collected already, don't do anything
        if(collected)
        {
            return;
        }

        // Otherwise, continue with interaction
        if(other.CompareTag("Player"))
        {
            // Mark it as collected
            collected = true;

            // Increase permanent upgrade count
            GameSession.Instance.AddHealthUpgrade(id);

            // Update player's current health immediately
            PlayerStats playerStats = other.GetComponentInParent<PlayerStats>();

            if(playerStats != null)
            {
                playerStats.UpdateMaxHealth();
            }

            // Hide the upgrade without destroying it
            UpdateCollectedState();
        }
    }

    private void UpdateCollectedState()
    {
        if(visual != null)
        {
            visual.enabled = !collected;
        }

        if(pickupCollider != null)
        {
            pickupCollider.enabled = !collected;
        }
    }
}