using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour
{
    private SpriteRenderer playerSprite;
    private PlayerMovement playerMovement;
    private Rigidbody2D rb;
    private Collider2D playerCollider;

    private void Awake()
    {
        // Grab current component values of player
        playerMovement = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();
        playerSprite = GetComponentInChildren<SpriteRenderer>();
        playerCollider = GetComponent<Collider2D>();
    }

    // Called when player dies
    public void HandleDeath()
    {
        // Prevent player from moving/interacting with anything after death
        rb.linearVelocity = Vector2.zero;
        playerMovement.enabled = false;
        playerSprite.enabled = false;
        playerCollider.enabled = false;
        rb.simulated = false;
    }

    // Reload the scene at the respawn point
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
