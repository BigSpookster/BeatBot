using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable, IEnemyMovable
{
    [field: SerializeField] public float MaxHealth {get; set;}

    public float CurrentHealth {get; set;}

    public Rigidbody2D RB {get; set;}

    public bool IsFacingRight {get; set;} = false;

    /* State Machine Variables */

    public EnemyStateMachine StateMachine {get; set;}
    public EnemyIdleState IdleState {get; set;}
    public EnemyChaseState ChaseState {get; set;}
    public EnemyAttackState AttackState {get; set;}
    
    private void Awake()
    {
        StateMachine = new EnemyStateMachine();

        IdleState = new EnemyIdleState(this, StateMachine);
        ChaseState = new EnemyChaseState(this, StateMachine);
        AttackState = new EnemyAttackState(this, StateMachine);

    }

    private void Start()
    {
        CurrentHealth = MaxHealth;

        RB = GetComponent<Rigidbody2D>();

        StateMachine.Initialize(IdleState);

    }

    private void Update()
    {
        StateMachine.CurrentEnemyState.FrameUpdate();

    }

    private void FixedUpdate()
    {
        StateMachine.CurrentEnemyState.PhysicsUpdate();

    }

    /* Animation Triggers */

    public enum AnimationTriggerType
    {
        EnemyDamaged,
        PlayFootstepSound

    }

    private void AnimationTriggerEvent(AnimationTriggerType triggerType)
    {
        StateMachine.CurrentEnemyState.AnimationTriggerEvent(triggerType);

    }


    /* Damageable Functions: */

    public void Damage(float damageAmount)
    {
        CurrentHealth -= damageAmount;

        if (CurrentHealth <= 0f)
            Die();

    }

    public void Die()
    {
        Destroy(gameObject);

    }


    /* Movement Functions: */

    public void MoveEnemy(Vector2 velocity)
    {
        

    }

    public void CheckForLeftOrRightFacing(Vector2 velocity)
    {
        if (IsFacingRight && velocity.x < 0f)
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 180f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            IsFacingRight = !IsFacingRight;

        }

        else if (!IsFacingRight && velocity.x > 0f)
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 0f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            IsFacingRight = !IsFacingRight;

        }

    }

}
