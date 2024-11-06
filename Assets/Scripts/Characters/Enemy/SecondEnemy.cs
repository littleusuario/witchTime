using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SecondEnemy : Enemy
{
    GameObject player;
    private GameObject enemy;
    [SerializeField] private float velocity;
    [SerializeField] private float StepDistance = 0.5f;
    [SerializeField] CollisionController hitboxEnemy;
    [SerializeField] float invincibilityFrames = 0.5f;
    [SerializeField] Animator animator;
    [SerializeField] PulseToTheBeat pulseToTheBeat;
    [SerializeField] float thresholdAttackDistance = 1.5f;
    bool death;
    [SerializeField] float distance = 0;
    public override event Action Ondie;
    EnemyStateManager stateManager;

    public float _StepDistance => StepDistance;
    public GameObject Player => player;
    public Animator Animator => animator;
    public bool Death { get => death; set => death = value; }
    public GameObject Enemy => enemy;
    [SerializeField] SpriteRenderer spriteRenderer = null;

    private Material material;
    [SerializeField] private Color flashColor = Color.white;
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

        State_SlimeNormal state_SlimeNormal = new State_SlimeNormal(this);
        State_SlimeDeath state_SlimeDeath = new State_SlimeDeath(this);
        stateManager = new EnemyStateManager(state_SlimeNormal, state_SlimeDeath);

    }

    private void Update()
    {
        distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance <= thresholdAttackDistance) 
        {
            Debug.Log("DangerZone");
            DamagaZone();
        }
    }

    public override void RunBehaviour()
    {
        stateManager.UpdateState();
    }

    public override void DamagaZone()
    {

    }

    public override void TakeDamage()
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

