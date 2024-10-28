using System;
using System.Collections;
using UnityEngine;

public class FirstEnemy : Enemy
{
    GameObject player;
    private GameObject enemy;
    [SerializeField] private float velocity;
    private float StepDistance = 0.75f;
    [SerializeField] CollisionController hitboxEnemy;
    [SerializeField] float invincibilityFrames = 0.5f;
    [SerializeField] Animator animator;
    [SerializeField] PulseToTheBeat pulseToTheBeat;
    [SerializeField] float thresholdAttackDistance;
    bool death;
    [SerializeField] float distance = 0;
    public override event Action Ondie;
    EnemyStateManager stateManager;

    [ColorUsage(true, true)]
    [SerializeField] private Color flashColor = Color.white;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private Material material;

    public float _StepDistance => StepDistance;
    public GameObject Player => player;
    public Animator Animator => animator;
    public bool Death { get => death; set => death = value; }
    public GameObject Enemy => enemy;

    private void Awake()
    {
        if (hitboxEnemy != null)
        {
            hitboxEnemy.CollisionTrigger += TakeDamage;
        }

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
        if (player != null)
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
        stateManager.UpdateState();
    }

    public override void DamagaZone()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, AttackRadius, transform.forward);

        foreach (RaycastHit hit in hits)
        {
            GameObject hitObject = hit.collider.transform.gameObject;

            if (hitObject.CompareTag("Player") && HealthPoints <= 0)
            {
                hitObject.GetComponent<PlayerHealth>().TakeDamage(1);
            }
        }
    }

    public override void TakeDamage()
    {
        if (HealthPoints <= 0) return;

        HealthPoints--;
        Debug.Log($"HealthPoints: {HealthPoints}, animator: {animator}, hitboxEnemy: {hitboxEnemy}");

        if (HealthPoints > 0)
        {
            if (animator != null)
            {
                StartCoroutine(HitEffect());
            }
        }
        else
        {
            death = true;
            if (animator != null)
            {
                animator.SetBool("Death", true);
            }
            StartCoroutine(InvincibilityFrames());
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
