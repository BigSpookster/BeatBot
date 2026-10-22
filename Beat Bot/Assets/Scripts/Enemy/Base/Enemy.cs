using UnityEngine;
using FMODUnity;

public class Enemy : MonoBehaviour, IDamageable, IEnemyMovable, ITriggerCheckable
{
    protected Transform playerTransform;

    [field: SerializeField] public float MaxHealth {get; set;}

    public float CurrentHealth {get; set;}

    public Rigidbody2D RB {get; set;}

    [SerializeField] protected int attackInterval = 1;
    protected int nextAttack = 0;

    /*  The following code is for using the markers,
        which I'm unsure is useful right now
    [SerializeField] private bool waitForString = false;
    [SerializeField] private string stringToWaitFor;
    */

    public bool IsFacingRight {get; set;} = false;

    public bool IsAggroed {get; set;}

    public bool IsWithinStrikingDistance {get; set;}
    

    /* State Machine Variables */

    public EnemyStateMachine StateMachine {get; set;}
    public EnemyIdleState IdleState {get; set;}
    public EnemyChaseState ChaseState {get; set;}
    public EnemyAttackState AttackState {get; set;}

    /* Idle Variables */
    public float MovementSpeed = 1f;

    public Transform[] patrolPoints;

    /* Chase Variables */
    public float AggroedMoveSpeed = 1.75f;

    /* Attack State Variables */
    public bool InAttackState = false;
    
    protected virtual void Awake()
    {
        nextAttack = attackInterval - 1;

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
            playerTransform = player.transform;

        StateMachine = new EnemyStateMachine();

        IdleState = new EnemyIdleState(this, StateMachine);
        ChaseState = new EnemyChaseState(this, StateMachine);
        AttackState = new EnemyAttackState(this, StateMachine);

        MusicManager.beatUpdated += Attack;

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
        StateMachine.CurrentEnemyState.FrameUpdate();

        if(Input.GetKeyDown(KeyCode.J))
        {
            Damage(1f);
            Debug.Log("Enemy health: " + CurrentHealth);
        }

    }

    private void FixedUpdate()
    {
        StateMachine.CurrentEnemyState.PhysicsUpdate();

    }

    // A base enemy has no specific attack
    protected virtual void Attack() {}

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

    public virtual void MoveEnemy(Vector2 normalizedVelocity, float speed) {}

    public void CheckForLeftOrRightFacing(Vector2 direction)
    {
        if (IsFacingRight && direction.x < 0f)
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 180f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            IsFacingRight = !IsFacingRight;

        }

        else if (!IsFacingRight && direction.x > 0f)
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 0f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            IsFacingRight = !IsFacingRight;

        }

    }

    private void OnDestroy()
    {
        // Stop managing the music
        MusicManager.beatUpdated -= Attack;
        
    }

}
