using UnityEngine;

public class SecondEnemy : Enemy
{
    [Header("Enemy Properties")]
    [SerializeField] private float velocity;
    [SerializeField] private float StepDistance = 0.5f;
    [SerializeField] float invincibilityFrames = 0.5f;

    public float _StepDistance => StepDistance;
    public GameObject Player => player;
    public Animator Animator => animator;
    public bool Death { get => death; set => death = value; }
    public GameObject Enemy => enemy;

    private void Start()
    {
        InitializeEnemy(spriteRenderer);

        State_SlimeNormal state_SlimeNormal = new State_SlimeNormal(this);
        State_SlimeDeath state_SlimeDeath = new State_SlimeDeath(this);
        stateManager = new EnemyStateManager(state_SlimeNormal, state_SlimeDeath);
    }

    private void Update()
    {
        EnemyUpdate();
    }
}

