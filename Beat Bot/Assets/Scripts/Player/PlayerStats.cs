using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    // Fields that take from other scripts
    [SerializeField] private DeathScreenUI deathScreenUI;
    [SerializeField] private PlayerDeath playerDeath;

    // Variable fields that can be edited in Unity
    [SerializeField] private float baseMaxHealth;
    [SerializeField] private float healthPerUpgrade;
    [SerializeField] private float takeDamageCooldown;

    // Variables
    private float maxHealth;
    private float nextDamageTime = 0f;
    private float currentHealth;
    private float deathState = 0;

    // Death State & Health Bar
    private bool isDeadYet = false;
    public HealthBar healthBar;
    
    private void Start() 
    {
        // Set Player's health to max at the start of the game (Each upgrade increases health by 1)
        maxHealth = baseMaxHealth + (healthPerUpgrade * GameSession.Instance.HealthUpgradesCollected);
        currentHealth = maxHealth;
        healthBar.SetSliderMax(maxHealth);
    }

    // Function - Updates Player's health when they take damage
    public void TakeDamage(float amount)
    {
        // If Statement - Check if damage was already taken
        if(Time.time < nextDamageTime)
        {
            return;
        }

        currentHealth -= amount;
        healthBar.SetSlider(currentHealth);

        // Player cannot take damage for 1 second
        nextDamageTime = Time.time + takeDamageCooldown;
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        healthBar.SetSlider(currentHealth);
    }

    private void Update()
    {
        // If Statement - Max Health Check (If player is already at max health, don't heal above)
        if(currentHealth > maxHealth) 
        {
            currentHealth = maxHealth;
        }

        // If Statement - Death State (If player health reaches 0, player is dead)
        if(currentHealth <= deathState && !isDeadYet) {
            Die();
        }
    }

    private void Die() 
    {
        // Change death state to true
        isDeadYet = true;

        // Deactivate player
        playerDeath.HandleDeath();

        // Show death screen
        deathScreenUI.ShowDeathScreen();
    }

}
