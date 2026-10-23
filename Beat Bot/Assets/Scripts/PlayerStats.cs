using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    // Player's Maximum Health
    [SerializeField] private float maxHealth;

    // How Long Player is Invincible after taking damage
    [SerializeField] private float takeDamageCooldown;

    // Time When Player can take damage again
    private float nextDamageTime = 0f;

    // Player's Current Health
    private float currentHealth;

    // Player Loses All Health
    private float deathState = 0;

    // Reference to HealthBar script
    public HealthBar healthBar;

    private void Start() 
    {
        // Set Player's health to max at the start of the game
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
        // Update health, update player health in GameSession, then update health bar
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        GameSession.Instance.SetPlayerHealth(currentHealth);
        healthBar.SetSlider(currentHealth);
    }

    private void Update()
    {
        // If Statement - Max Health Check (If player is already at max health, don't heal above)
        if(currentHealth > maxHealth) 
        {
            currentHealth = maxHealth;
        }

        // If Statement - Damage Test (Press K to take damage)
        if(Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage(20f);
        }

        // If Statement - Heal Test (Press H to heal player)
        if(Input.GetKeyDown(KeyCode.H))
        {
            Heal(20f);
        }

        // If Statement - Death State (If player health reaches 0, player is dead)
        if(currentHealth <= deathState) {
            Die();
        }
    }

    private void Die() 
    {
        Debug.Log("You have died!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
