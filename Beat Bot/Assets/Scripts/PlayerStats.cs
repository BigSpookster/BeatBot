using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private float takeDamageCooldown;
    [SerializeField] private DeathScreenUI deathScreenUI;
    [SerializeField] private PlayerDeath playerDeath;
    [SerializeField] private GameSession gameSession;

    private float maxHealth;
    private float nextDamageTime = 0f;
    private float currentHealth;
    private float deathState = 0;
    private bool isDeadYet = false;
    public HealthBar healthBar;
    

    private void Start() 
    {
        // Set Player's health to max at the start of the game
        maxHealth = gameSession.CalcualteHealth();
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

        // If Statement - Damage Test (Press K to take damage)
        if(Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage(1f);
        }

        // If Statement - Heal Test (Press H to heal player)
        if(Input.GetKeyDown(KeyCode.H))
        {
            Heal(1f);
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
