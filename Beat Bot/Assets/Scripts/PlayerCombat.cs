using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    //General Combat Fields
    [SerializeField] private InputActionReference attackInput;
    [SerializeField] private int maxCombo = 3;
    [SerializeField] private float comboResetTime = 0.8f;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject attackHitbox;
    [SerializeField] private PlayerStats playerStats;

    //General Combat Variables
    private int currentCombo = 0;
    private float comboTimer = 0f;
    private bool isAttacking = false;
    private bool canQueueNextAttack = false;
    private bool attackQueued = false;
    private bool hasAirAttacked = false;
    private Rigidbody2D rb;
    private PlayerMovement movement;
    private WeaponManager weaponManager;


    //Weapon 2 Serialize Field Stuff
    [SerializeField] private GameObject weapon2ProjectilePrefab;
    [SerializeField] private Transform weapon2FirePoint;
    [SerializeField] private float weapon2AttackCooldown = 0.3f;
    [SerializeField] private int weapon2MagazineSize = 5;
    [SerializeField] private int weapon2AirShotLimit = 3;
    [SerializeField] private float weapon2ReloadTime = 1.5f;
    [SerializeField] private float weapon2IdleReloadDelay = 2.5f;

    //Weapon 2 Variables
    private float weapon2AttackTimer = 0f;
    private int weapon2CurrentAmmo;
    private int weapon2AirShots;
    private float weapon2ReloadTimer;
    private float weapon2IdleTimer = 0f;
    private bool weapon2IsReloading = false;


    // Weapon 3 Serialize Field Ground Stuff
    [SerializeField] private GameObject weapon3SwordHitbox;
    [SerializeField] private float weapon3ComboWindow = 0.5f;
    [SerializeField] private float chargeStartThreshold = 0.5f;
    [SerializeField] private float maxChargeTime = 1f;

    //Weapon 3 Serialize Field Air Stuff
    [SerializeField] private float weapon3DiveSpeed = 9f;
    [SerializeField] private float weapon3AirPauseDuration = 0.25f;

    //Weapon 3 Variables
    private float weapon3ChargeTimer = 0f;
    private bool isChargingWeapon3 = false;
    private bool isWeapon3AttackPending = false;
    private bool weapon3Attack1Used = false;
    private float weapon3AirPauseTimer = 0f;
    private float originalGravityScale;
    private bool isWeapon3AirPausing = false;
    private bool isWeapon3AirAttacking = false;

    //Weapon 3 Ability Stuff
    [SerializeField] private float shieldDuration = 1f;
    [SerializeField] private float shieldCooldown = 5f;
    private float shieldTimer = 0f;
    private float shieldCooldownTimer = 0f;
    private bool isShieldActive = false;
    private float shieldAnimationTimer = 0.3f;

    //General Combat Methods
    private void Start()
    {
        weapon2CurrentAmmo = weapon2MagazineSize;
        weapon2AirShots = 0;
    }

    private void Awake()
    {
        rb = GetComponentInParent<Rigidbody2D>();
        movement = GetComponent<PlayerMovement>();
        weaponManager = GetComponent<WeaponManager>();
        playerStats = FindFirstObjectByType<PlayerStats>();
        originalGravityScale = rb.gravityScale;
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
        if (isWeapon3AirPausing)
        {
            weapon3AirPauseTimer -= Time.deltaTime;

            rb.linearVelocity = Vector2.zero;

            if (weapon3AirPauseTimer <= 0f)
            {
                isWeapon3AirPausing = false;
                rb.gravityScale = originalGravityScale;
                rb.linearVelocity = new Vector2(0f, -weapon3DiveSpeed);
            }
        }

        if (isWeapon3AirAttacking && movement.IsGrounded)
        {
            isWeapon3AirAttacking = false;
            isWeapon3AirPausing = false;
            rb.gravityScale = originalGravityScale;
            isAttacking = false;

            weapon3SwordHitbox.SetActive(false);
            movement.SetMovementLocked(false);

            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);

            currentCombo = 0;

            if (animator != null)
                animator.Play("Idle", 0, 0f);
        }

        //Resets Air Attacks and Handles Weapon 2 Landing Ammo.
        if (movement.IsGrounded)
        {
            hasAirAttacked = false;

            if (weapon2AirShots >= weapon2AirShotLimit)
            {
                weapon2CurrentAmmo = 0;
                weapon2IdleTimer = 0f;
            }

            weapon2AirShots = 0;
        }

        if (isShieldActive)
        {
            shieldTimer -= Time.deltaTime;
            shieldAnimationTimer -= Time.deltaTime;

            if (shieldAnimationTimer <= 0f && animator != null)
            {
                animator.speed = 0f;
            }

            if (shieldTimer <= 0f)
            {
                isShieldActive = false;

                if (animator != null)
                {
                    animator.speed = 1f;
                    animator.Play("Idle", 0, 0f);
                }
            }
        }

        if (shieldCooldownTimer > 0f)
        {
            shieldCooldownTimer -= Time.deltaTime;
        }

        if (weapon2AttackTimer > 0f)
        {
            weapon2AttackTimer -= Time.deltaTime;

            if (weapon2AttackTimer <= 0f)
            {
                isAttacking = false;
            }
        }

        //Used For Weapon 2 Automatic Reload
        if (weapon2IsReloading)
        {
            weapon2ReloadTimer -= Time.deltaTime;

            if (weapon2ReloadTimer <= 0f)
            {
                weapon2CurrentAmmo = weapon2MagazineSize;
                weapon2AirShots = 0;
                weapon2IsReloading = false;
                weapon2IdleTimer = 0f;
            }
        }
        else if (weapon2CurrentAmmo <= 0)
        {
            weapon2IsReloading = true;
            weapon2ReloadTimer = weapon2ReloadTime;
        }
        else if (weapon2CurrentAmmo < weapon2MagazineSize)
        {
            weapon2IdleTimer -= Time.deltaTime;

            if (weapon2IdleTimer <= 0f)
            {
                weapon2CurrentAmmo = weapon2MagazineSize;
                weapon2IdleTimer = 0f;
            }
        }

        if (attackInput == null)
            return;

        if (isShieldActive)
            return;

        if (weaponManager.currentWeapon == WeaponType.Weapon3 && movement.IsGrounded)
        {
            if (attackInput.action.WasPressedThisFrame())
                HandleWeapon3ChargeInput();

            if (isChargingWeapon3 || isWeapon3AttackPending)
                UpdateWeapon3Charge();
        }
        else if (attackInput.action.WasPressedThisFrame())
        {
            HandleAttackInput();
        }

        //Counts down the combo timer when not actively attacking or charging so the combo resets
        if (!isAttacking && !isWeapon3AttackPending && !isChargingWeapon3 && currentCombo > 0)
        {
            comboTimer -= Time.deltaTime;

            if (comboTimer <= 0f)
            {
                currentCombo = 0;
                weapon3Attack1Used = false;
            }
        }
    }

    private void HandleAttackInput()
    {
        if (!isAttacking)
        {
            if (!movement.IsGrounded)
            {
                if (weaponManager.currentWeapon == WeaponType.Weapon1 && !hasAirAttacked)
                    StartAirAttack();
                else if (weaponManager.currentWeapon == WeaponType.Weapon2)
                    StartAttack();
                else if (weaponManager.currentWeapon == WeaponType.Weapon3 && !hasAirAttacked)
                    StartWeapon3AirAttack();

                return;
            }

            if (comboTimer <= 0f)
                currentCombo = 0;

            if (weaponManager.currentWeapon == WeaponType.Weapon3)
            {
                StartWeapon3Attack();
            }
            else
            {
                StartAttack();
            }
        }
        else if (movement.IsGrounded && canQueueNextAttack && currentCombo < maxCombo)
        {
            attackQueued = true;
        }
    }

    //Ground Attack Stuff For Weapon 1 and 2
    private void StartAttack()
    {
        if (weaponManager.currentWeapon == WeaponType.Weapon2 &&
            (weapon2IsReloading || weapon2CurrentAmmo <= 0))
            return;

        if (weaponManager.currentWeapon == WeaponType.Weapon2 &&
            !movement.IsGrounded &&
            weapon2AirShots >= weapon2AirShotLimit)
            return;

        currentCombo++;

        if (currentCombo > maxCombo)
            currentCombo = 1;

        isAttacking = true;
        canQueueNextAttack = false;
        attackQueued = false;
        comboTimer = comboResetTime;

        if (animator == null || movement == null)
            return;

        bool attackRight = movement.FacingRight;
        string direction = attackRight ? "R" : "L";

        if (weaponManager.currentWeapon == WeaponType.Weapon1)
        {
            animator.Play("Attack" + direction + currentCombo, 0, 0f);
        }

        if (weaponManager.currentWeapon == WeaponType.Weapon2)
        {
            weapon2CurrentAmmo--;

            if (!movement.IsGrounded)
            {
                weapon2AirShots++;
            }

            weapon2IdleTimer = weapon2IdleReloadDelay;

            weapon2AttackTimer = weapon2AttackCooldown;
            FireWeapon2Projectile();
        }
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
        isAttacking = false;
        attackQueued = false;
        movement.SetMovementLocked(false);

        if (weaponManager.currentWeapon == WeaponType.Weapon3)
        {
            if (currentCombo == 1)
            {
                //If Attack 1 has finished, start window to do attack 2
                comboTimer = weapon3ComboWindow;
            }
            else if (currentCombo == 2)
            {
                //If Attack 2 has finished, reset the Weapon 3 combo.
                currentCombo = 0;
                weapon3Attack1Used = false;
                comboTimer = 0f;
            }
        }
        else
        {
            comboTimer = comboResetTime;
        }

        if (animator != null)
            animator.Play("Idle", 0, 0f);
    }

    public int GetCurrentCombo()
    {
        return currentCombo;
    }

    public float GetCurrentAttackDamage()
    {
        switch (weaponManager.currentWeapon)
        {
            case WeaponType.Weapon1:
                if (currentCombo == 1 || currentCombo == 2)
                    return 3f;
                if (currentCombo == 3)
                    return 5f;
                break;

            case WeaponType.Weapon2:
                break;

            case WeaponType.Weapon3:
                if (currentCombo == 1)
                    return 5f;
                if (currentCombo == 2)
                    return 10f;
                break;
        }

        return 3f;
    }


    //Weapon 1 Combat Methods
    public void EnableAttackHitbox()
    {
        attackHitbox.SetActive(true);
    }

    public void DisableAttackHitbox()
    {
        attackHitbox.SetActive(false);
    }

    private void StartAirAttack()
    {
        hasAirAttacked = true;
        isAttacking = true;
        canQueueNextAttack = false;
        attackQueued = false;
        comboTimer = comboResetTime;

        currentCombo = 0;
        comboTimer = 0f;

        if (animator == null || movement == null)
            return;

        string direction = movement.FacingRight ? "R" : "L";
        animator.Play("AAW1" + direction, 0, 0f);
    }


    //Weapon 2 Combat Methods
    private void FireWeapon2Projectile()
    {
        if (weapon2ProjectilePrefab == null || weapon2FirePoint == null)
            return;

        Vector2 direction = movement.FacingRight ? Vector2.right : Vector2.left;

        Vector3 spawnPosition = weapon2FirePoint.position;

        if (!movement.FacingRight)
        {
            float offset = Mathf.Abs(weapon2FirePoint.localPosition.x);
            spawnPosition.x = transform.position.x - offset;
        }

        GameObject projectile = Instantiate(
            weapon2ProjectilePrefab,
            spawnPosition,
            Quaternion.identity
        );

        projectile.GetComponent<Weapon2Projectile>().Initialize(direction);
    }


    //Weapon 3 Combat Methods
    private void StartWeapon3Attack()
    {
        currentCombo = 1;
        isAttacking = true;
        movement.SetMovementLocked(true);
        canQueueNextAttack = false;
        attackQueued = false;

        if (animator == null || movement == null)
            return;

        string direction = movement.FacingRight ? "R" : "L";
        animator.Play("Weapon3A1" + direction, 0, 0f);
    }

    public void EnableWeapon3SwordHitbox()
    {
        weapon3SwordHitbox.SetActive(true);
    }

    public void DisableWeapon3SwordHitbox()
    {
        weapon3SwordHitbox.SetActive(false);
    }

    private void StartWeapon3AirAttack()
    {
        hasAirAttacked = true;
        isWeapon3AirAttacking = true;
        isAttacking = true;

        currentCombo = 1;
        canQueueNextAttack = false;
        attackQueued = false;

        movement.SetMovementLocked(true);

        isWeapon3AirPausing = true;
        weapon3AirPauseTimer = weapon3AirPauseDuration;

        rb.gravityScale = 0f;
        rb.linearVelocity = Vector2.zero;

        string direction = movement.FacingRight ? "R" : "L";
        animator.Play("Weapon3AA" + direction, 0, 0f);
    }

    private void HandleWeapon3ChargeInput()
    {
        if (isAttacking || isWeapon3AttackPending || isChargingWeapon3)
            return;

        //First attack in the combo.
        if (!weapon3Attack1Used)
        {
            weapon3Attack1Used = true;
            StartWeapon3Attack();
            return;
        }

        //Only allows Attack 2 during the specific window after Attack 1.
        if (comboTimer <= 0f)
            return;

        isWeapon3AttackPending = true;
        weapon3ChargeTimer = 0f;
    }

    private void UpdateWeapon3Charge()
    {
        //Window for player to charge attack 2
        if (isWeapon3AttackPending)
        {
            if (attackInput.action.WasReleasedThisFrame())
            {
                //A quick tap cancels the charge attempt.
                isWeapon3AttackPending = false;
                weapon3ChargeTimer = 0f;
                return;
            }

            weapon3ChargeTimer += Time.deltaTime;

            if (weapon3ChargeTimer >= chargeStartThreshold)
            {
                isWeapon3AttackPending = false;
                isChargingWeapon3 = true;
                isAttacking = true;

                weapon3ChargeTimer = 0f;
                movement.SetMovementLocked(true);

                string direction = movement.FacingRight ? "R" : "L";
                animator.Play("Weapon3A2Charge" + direction, 0, 0f);
            }

            return;
        }

        if (isChargingWeapon3)
        {
            weapon3ChargeTimer += Time.deltaTime;

            if (attackInput.action.WasReleasedThisFrame() ||
                weapon3ChargeTimer >= maxChargeTime)
            {
                StartWeapon3Release();
            }
        }
    }

    private void StartWeapon3Release()
    {
        isChargingWeapon3 = false;
        isWeapon3AttackPending = false;
        isAttacking = true;

        movement.SetMovementLocked(true);

        weapon3ChargeTimer = 0f;
        currentCombo = 2;

        string direction = movement.FacingRight ? "R" : "L";
        animator.Play("Weapon3A2Release" + direction, 0, 0f);
    }

    public void ActivateShield()
    {
        if (isShieldActive || shieldCooldownTimer > 0f)
            return;

        isShieldActive = true;
        shieldTimer = shieldDuration;
        shieldCooldownTimer = shieldCooldown;

        animator.speed = 1f;
        shieldAnimationTimer = 0.25f;
        animator.Play("Shield", 0, 0f);
    }
}
