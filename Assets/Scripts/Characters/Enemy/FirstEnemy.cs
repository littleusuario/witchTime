using System;
using System.Collections;
using UnityEngine;

public class FirstEnemy : Enemy
{
    [Header("Enemy Properties")]
    [ColorUsage(true, true)]
    [SerializeField] private Color flashColor = Color.white;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] CollisionController hitboxEnemy;
    [SerializeField] float invincibilityFrames = 0.5f;
    [SerializeField] Animator animator;
    [SerializeField] PulseToTheBeat pulseToTheBeat;
    [SerializeField] float thresholdAttackDistance;
    [SerializeField] private float velocity;
    [SerializeField] float distance = 0;
    
    private GameObject player;
    private GameObject enemy;
    private float StepDistance = 0.75f;
    private bool death;
    private EnemyStateManager stateManager;
    private Material material;

    public override event Action Ondie;
    public float _StepDistance => StepDistance;
    public GameObject Player => player;
    public Animator Animator => animator;
    public bool Death { get => death; set => death = value; }
    public GameObject Enemy => enemy;

    private void Awake()
    {
        if (pulseToTheBeat != null)
        {
            pulseToTheBeat.beatPulse += RunBehaviour;
        }
    }

    private void Start()
    {
        if (spriteRenderer != null)
        {
            material = spriteRenderer.material;
        }
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject;
        }

        enemy = gameObject.transform.parent.gameObject;

        State_SkeletonNormal state_SkeletonNormal = new State_SkeletonNormal(this);
        State_SkeletonDeath state_SkeletonDeath = new State_SkeletonDeath(this);
        stateManager = new EnemyStateManager(state_SkeletonNormal, state_SkeletonDeath);

        HealthPoints = 3;
    }

    private void Update()
    {
        if (player != null && beats > beatsInactiveStart)
        {
            distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance <= thresholdAttackDistance)
            {
                DamagaZone();
            }
        }
    }

    public override void RunBehaviour()
    {
        if(beats >= beatsInactiveStart)
            stateManager.UpdateState();

        beats++;
    }

    public override void DamagaZone()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, AttackRadius, transform.forward);

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.GetComponent<Enemy>() == null) 
            {
                IDamageable damageableObject = hit.collider.gameObject.GetComponent<IDamageable>();

                if (damageableObject != null && HealthPoints > 0)
                {
                    damageableObject.TakeDamage(1);
                }
            }
        }
    }

    public override void TakeDamage(int damage)
    {
        if (HealthPoints <= 0) return;

        HealthPoints--;
        Debug.Log($"HealthPoints: {HealthPoints}, animator: {animator}, hitboxEnemy: {hitboxEnemy}");

        if (HealthPoints > 0)
        {
            GameObject particles = Instantiate(DamageParticleSystem, transform.position + transform.up, Quaternion.identity).gameObject;

            if (animator != null)
            {
                StartCoroutine(HitEffect());
            }
        }
        else
        {
            death = true;
            Destroy(gameObject.transform.parent.gameObject.transform.parent.gameObject);
            //if (animator != null)
            //{
            //    animator.SetBool("Death", true);
            //}
            //StartCoroutine(InvincibilityFrames());
        }

        if (HealthPoints == 0 && Ondie != null)
        {
            Ondie.Invoke();
        }
    }

    IEnumerator InvincibilityFrames()
    {
        hitboxEnemy.enabled = false;
        yield return new WaitForSeconds(2f);

        if (death)
        {
            Destroy(gameObject.transform.parent.gameObject.transform.parent.gameObject);
        }
        else
        {
            hitboxEnemy.enabled = true;
        }
    }

    private void OnDestroy()
    {
        if (pulseToTheBeat != null)
        {
            pulseToTheBeat.beatPulse -= RunBehaviour;
            pulseToTheBeat.enabled = false;
        }
    }

    private IEnumerator HitEffect()
    {
        float elapsedTime = 0f;
        float currentFlashAmount = 0f;

        material.SetColor("_FlashColor", flashColor);

        Color originalColor = spriteRenderer.color;
        spriteRenderer.color = Color.red;

        while (elapsedTime < 0.25f)
        {
            elapsedTime += Time.deltaTime;

            float lerpFactor = elapsedTime / 0.25f;
            currentFlashAmount = Mathf.Lerp(1f, 1f, lerpFactor);
            StartCoroutine(InvincibilityFrames());
            material.SetFloat("_FlashAmount", currentFlashAmount);

            yield return null;
        }

        spriteRenderer.enabled = true;
        spriteRenderer.color = originalColor;
        material.SetFloat("_FlashAmount", 0f);
    }
}
