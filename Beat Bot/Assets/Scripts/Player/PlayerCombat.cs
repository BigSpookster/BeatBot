using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    [Header("Attack Input")]
    [SerializeField] private InputActionReference attackInput;

    [Header("Combo Settings")]
    [SerializeField] private int maxCombo = 3;
    [SerializeField] private float comboResetTime = 0.8f;

    [Header("Animation")]
    [SerializeField] private Animator animator;

    private PlayerMovement movement;
    [SerializeField] private GameObject attackHitbox;


    private int currentCombo = 0;
    private float comboTimer = 0f;
    private bool isAttacking = false;
    private bool canQueueNextAttack = false;
    private bool attackQueued = false;

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
    }

    private void OnEnable()
    {
        if (attackInput != null)
            attackInput.action.Enable();
    }

    private void OnDisable()
    {
        if (attackInput != null)
            attackInput.action.Disable();
    }

    private void Update()
    {
        if (attackInput == null)
            return;

        if (attackInput.action.WasPressedThisFrame())
            HandleAttackInput();

        if (!isAttacking && currentCombo > 0)
        {
            comboTimer -= Time.deltaTime;

            if (comboTimer <= 0f)
                currentCombo = 0;
        }
    }

    private void HandleAttackInput()
    {
        if (!isAttacking)
        {
            if (comboTimer <= 0f)
                currentCombo = 0;

            StartAttack();
        }
        else if (canQueueNextAttack && currentCombo < maxCombo)
        {
            attackQueued = true;
        }
    }

    private void StartAttack()
    {
        currentCombo++;

        if (currentCombo > maxCombo)
            currentCombo = 1;

        isAttacking = true;
        canQueueNextAttack = false;
        attackQueued = false;
        comboTimer = comboResetTime;

        if (animator == null || movement == null)
            return;

        // Use the same direction for the attack and its lunge.
        // bool attackRight = movement.FacingRight;
        // string direction = attackRight ? "R" : "L";

        // animator.Play("Attack" + direction + currentCombo, 0, 0f);

        // if (currentCombo == 3)
            // movement.LungeForward(attackRight);
    }

    public void OpenComboWindow()
    {
        canQueueNextAttack = true;
    }

    public void CloseComboWindow()
    {
        canQueueNextAttack = false;
    }

    public void EndAttack()
    {
        canQueueNextAttack = false;

        if (attackQueued && currentCombo < maxCombo)
        {
            attackQueued = false;
            StartAttack();
            return;
        }

        isAttacking = false;
        attackQueued = false;
        comboTimer = comboResetTime;

        if (animator != null)
            animator.Play("Idle", 0, 0f);
    }

    public int GetCurrentCombo()
    {
        return currentCombo;
    }
    public void EnableAttackHitbox()
    {
        attackHitbox.SetActive(true);
    }

    public void DisableAttackHitbox()
    {
        attackHitbox.SetActive(false);
    }
}
