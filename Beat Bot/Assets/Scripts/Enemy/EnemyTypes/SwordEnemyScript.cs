using UnityEngine;

public class SwordEnemyScript : Enemy
{
    private CapsuleCollider2D swordCollider = null;
    private SpriteRenderer swordSprite = null;

    [SerializeField] private int attackInterval;
    private int nextAttack = 0;


    /*  The following code is for using the markers,
        which I'm unsure is useful right now
    [SerializeField] private bool waitForString = false;
    [SerializeField] private string stringToWaitFor;
    */

    private void Awake()
    {
        nextAttack = attackInterval - 1;

        // Finding the correct child
        Transform checkChildTranform = transform.Find("Sword");

        // If it exists, get the sword's collider and sprite
        if (checkChildTranform != null)
        {
            swordCollider = checkChildTranform.GetComponent<CapsuleCollider2D>();
            swordSprite = checkChildTranform.GetComponent<SpriteRenderer>();

        }

        // Otherwise, there might be an error
        else
            Debug.Log("Error: sword child was not found...");

        // Music Managment
        MusicManager.beatUpdated += Attack;
        
    }

    private void OnDestroy()
    {
        // Stop managing the music
        MusicManager.beatUpdated -= Attack;
        
    }

    private void Attack()
    {
        // If we have not reached the next attack, decrement
        if (nextAttack > 0)
        {
            nextAttack--;

            // If the sword is enabled, we want to disable it
            if (swordCollider.enabled)
                DisableSword();

        }

        // Otherwise, toggle the sword and reset nextAttack
        else
        {
            EnableSword();
            nextAttack = attackInterval - 1;

        }

    }

    // Enables the sword's sprite and collider
    private void EnableSword()
    {
        swordCollider.enabled = true;
        swordSprite.enabled = true;

    }

    // Disables the sword's sprite and collider
    private void DisableSword()
    {
        swordCollider.enabled = false;
        swordSprite.enabled = false;

    }

}
