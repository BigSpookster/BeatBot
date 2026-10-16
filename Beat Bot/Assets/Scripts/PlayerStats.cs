using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float takeDamageCooldown;
    private float nextDamageTime = 0f;
    private float currentHealth;
    private float deathState = 0;
    public HealthBar healthBar;
    [SerializeField] private GameObject deathScreen;
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private CanvasGroup deathText;
    [SerializeField] private float deathTextFadeTime;
    private float elapsedTime;
    private bool isDeadYet = false;

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
            TakeDamage(20f);
        }

        // If Statement - Heal Test (Press H to heal player)
        if(Input.GetKeyDown(KeyCode.H))
        {
            Heal(20f);
        }

        // If Statement - Death State (If player health reaches 0, player is dead)
        if(currentHealth <= deathState && !isDeadYet) {
            Die();
        }
    }

    private void Die() 
    {
        isDeadYet = true;

        deathScreen.SetActive(true);
        deathText.alpha = 0f;

        StartCoroutine(FadeInDeathText());

        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private IEnumerator FadeInDeathText() 
    {
        elapsedTime = 0f;

        while(elapsedTime < deathTextFadeTime)
        {
            elapsedTime += Time.deltaTime;

            deathText.alpha = Mathf.Lerp(0f, 1f, elapsedTime / deathTextFadeTime);

            yield return null;
        }

        deathText.alpha = 1f;
    }
}
