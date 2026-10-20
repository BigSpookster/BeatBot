
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    
    [Header("Attack Input")]
    [SerializeField] private InputActionReference attackInput;

    [Header("Combo Settings")]
    [SerializeField] private int maxCombo = 3;
    //Used to know what attack animation is next
    [SerializeField] private float comboResetTime = 0.8f;

    //Used to keep track of the current combo length/state
    private int currentCombo = 0;
    private float comboTimer = 0f;
    private bool isAttacking = false;
    private bool canQueueNextAttack = false;
    private bool attackQueued = false;

    //This runs when the combat script is in use.
    private void OnEnable()
    {
        //This runs if an attack is inputted by the player.
        if (attackInput != null)
            attackInput.action.Enable();
    }
    //This runs if the combat script is not in use or ends use.
    private void OnDisable()
    {
        if (attackInput != null)
            attackInput.action.Disable();
    }
    //This runs every frame to check for player input and manage the combo timer.
    private void Update()
    {
        if (attackInput == null)
            return;

        // Read attack input using Unity's new Input System.
        if (attackInput.action.WasPressedThisFrame())
        {
            HandleAttackInput();
        }

        // Reset the combo if too much time passes.
        if (!isAttacking && currentCombo > 0)
        {
            comboTimer -= Time.deltaTime;

            if (comboTimer <= 0f)
            {
                currentCombo = 0;
            }
        }
    }
    //This is used to decide what happens when the player presses the attack button.
    private void HandleAttackInput()
    {
        //If the player is not attacking/comboing, it starts from the beginning.
        if (!isAttacking)
        {
            StartAttack();
        }
        //If the player is in the middle of a combo, it loads the next attack.
        else if (canQueueNextAttack && currentCombo < maxCombo)
        {
            attackQueued = true;
        }
    }
    //This is used to start the next attack in the combo
    private void StartAttack()
    {
        currentCombo++;

        if (currentCombo > maxCombo)
            currentCombo = 1;

        isAttacking = true;
        canQueueNextAttack = false;
        attackQueued = false;
        comboTimer = comboResetTime;

        Debug.Log("Started Attack " + currentCombo);
    }

    //Allows the player to queue the next attack in the combo.
    public void OpenComboWindow()
    {
        canQueueNextAttack = true;
    }

    //Stops the player from being able to queue the next attack in the combo.
    public void CloseComboWindow()
    {
        canQueueNextAttack = false;
    }

    // Call this at the end of each attack animation.
    public void EndAttack()
    {
        isAttacking = false;
        canQueueNextAttack = false;

        if (attackQueued && currentCombo < maxCombo)
        {
            StartAttack();
        }
        else
        {
            attackQueued = false;
            comboTimer = comboResetTime;
        }
    }

    public int GetCurrentCombo()
    {
        return currentCombo;
    }
}
