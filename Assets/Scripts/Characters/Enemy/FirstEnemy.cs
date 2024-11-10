using UnityEngine;

public class FirstEnemy : Enemy
{
    [Header("Enemy Properties")]
    [ColorUsage(true, true)]

    [SerializeField] float invincibilityFrames = 0.5f;

    [SerializeField] private float velocity;

    private float StepDistance = 0.75f;

    public float _StepDistance => StepDistance;
    public GameObject Player => player;
    public Animator Animator => animator;
    public bool Death { get => death; set => death = value; }
    public GameObject Enemy => enemy;
    private void Start()
    {
        InitializeEnemy(spriteRenderer);

        State_SkeletonNormal state_SkeletonNormal = new State_SkeletonNormal(this);
        State_SkeletonDeath state_SkeletonDeath = new State_SkeletonDeath(this);
        stateManager = new EnemyStateManager(state_SkeletonNormal, state_SkeletonDeath);

        HealthPoints = 3;
    }

    private void Update()
    {
        EnemyUpdate();
    }
}
