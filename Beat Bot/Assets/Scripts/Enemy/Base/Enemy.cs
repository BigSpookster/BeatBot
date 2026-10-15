using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable, IEnemyMovable, ITriggerCheckable
{
    [field: SerializeField] public float MaxHealth {get; set;}

    public float CurrentHealth {get; set;}

    public Rigidbody2D RB {get; set;}

    public bool IsFacingRight {get; set;} = false;

    public bool IsAggroed {get; set;}

    public bool IsWithinStrikingDistance {get; set;}
    

    /* State Machine Variables */

    public EnemyStateMachine StateMachine {get; set;}
    public EnemyIdleState IdleState {get; set;}
    public EnemyChaseState ChaseState {get; set;}
    public EnemyAttackState AttackState {get; set;}

    /* Idle Variables */
    public float RandomMovementRange = 5f;
    public float RandomMovementSpeed = 1f;

    public Transform[] patrolPoints;

    /* Aggro Variables */
    public float AggroedMoveSpeed = 1.75f;
    
    protected virtual void Awake()
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


    /* Distance Check Functions */

    public void SetAggroStatus(bool isAggroed)
    {
        IsAggroed = isAggroed;

    }

    public void SetStrikingDistance(bool isWithinStrikingDistance)
    {
        IsWithinStrikingDistance = isWithinStrikingDistance;
        
    }


    /* Movement Functions: */

    public void MoveEnemy(Vector2 velocity)
    {
        RB.linearVelocity = new Vector2(velocity.x, RB.linearVelocity.y);
        CheckForLeftOrRightFacing(velocity);

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
