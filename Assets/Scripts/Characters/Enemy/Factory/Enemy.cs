using System;
using System.Collections;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IDamageable
{
    [Header("Enemy Parent Class")]
    public int HealthPoints;
    public int AttackRadius = 5;
    public event Action Ondie;
    public int dropChance;
    public ParticleSystem DamageParticleSystem;
    public float timeInactive = 1;
    public float elapsedTime = 0;
    public RoomObject originRoom;

    protected GameObject player;
    protected GameObject enemy;
    protected Material originalMaterial;
    protected float distance = 0;
    protected EnemyStateManager stateManager;

    [SerializeField] protected GameObject heart;
    [SerializeField] protected float thresholdAttackDistance = 1.5f;
    [SerializeField] protected Animator animator;
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] protected CollisionController hitboxEnemy;
    [SerializeField] protected PulseToTheBeat pulseToTheBeat;
    [SerializeField] protected Material hitMaterial;

    private GameManager gameManager;
    private bool isMaterialChanged = false;
    protected bool death;

    protected void InitializeEnemy(SpriteRenderer spriteRenderer)
    {
        gameManager = GameManager.Instance;
        originalMaterial = spriteRenderer.material;

        if (pulseToTheBeat != null)
        {
            pulseToTheBeat.beatPulse += RunBehaviour;
        }

        player = GameObject.FindGameObjectWithTag("Player");
        enemy = gameObject.transform.parent?.parent?.gameObject;
    }

    protected void EnemyUpdate()
    {
        elapsedTime += Time.deltaTime;

        if (player != null && elapsedTime > timeInactive)
        {
            distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance <= thresholdAttackDistance)
            {
                DamageZone();
            }
        }
    }

    public void RunBehaviour()
    {
        if (elapsedTime >= timeInactive)
        {
            stateManager?.UpdateState();
        }
    }

    protected void DamageZone()
    {
        var hits = Physics.SphereCastAll(transform.position, AttackRadius, transform.forward);

        foreach (var hit in hits)
        {
            if (hit.collider.GetComponent<Enemy>() == null)
            {
                var damageableObject = hit.collider.GetComponent<IDamageable>();

                if (damageableObject != null && HealthPoints > 0)
                {
                    damageableObject.TakeDamage(1);
                }
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (HealthPoints <= 0) return;

        gameManager.TriggerCameraShake(0.5f, 5f, 0.25f);
        gameManager.PauseGameForSeconds(0.09f);
        HealthPoints -= damage;

        if (gameManager.IsGamePaused && !isMaterialChanged)
        {
            spriteRenderer.material = hitMaterial;
            isMaterialChanged = true;
        }

        if (HealthPoints > 0)
        {
            //Instantiate(DamageParticleSystem, transform.position + transform.up, Quaternion.identity);
        }
        else
        {
            death = true;
            StartCoroutine(HandleDeath());

        }

        Ondie?.Invoke();
    }

    private IEnumerator HandleDeath()
    {
        yield return new WaitForSeconds(0.09f);
        Drop(dropChance, transform.position); //Drop
        Instantiate(DamageParticleSystem, transform.position + transform.up, Quaternion.identity);
        originRoom.EnemiestoSpawn.Remove(enemy);
        Destroy(enemy);
    }

    private void Update()
    {
        if (!gameManager.IsGamePaused && isMaterialChanged)
        {
            spriteRenderer.material = originalMaterial;
            isMaterialChanged = false;
        }
    }

    public void Drop(int dropChance, Vector3 spawnTransform)
    {
        int possibility = UnityEngine.Random.Range(0, 100);
        if (possibility <= dropChance && gameManager.playerCurrentHealth < gameManager.playerMaxHealth)
        {
            Instantiate(heart, spawnTransform + Vector3.up, Quaternion.identity);
        }
    }

    public void FlipTowardsPlayer()
    {
        if (player == null || spriteRenderer == null) return;

        spriteRenderer.flipX = player.transform.position.x > transform.position.x;
    }

    protected IEnumerator InvincibilityFrames()
    {
        hitboxEnemy.enabled = false;
        yield return new WaitForSeconds(2f);

        if (death)
        {
            Destroy(enemy);
        }
        else
        {
            hitboxEnemy.enabled = true;
        }
    }

    protected void OnDestroy()
    {
        if (pulseToTheBeat != null)
        {
            pulseToTheBeat.beatPulse -= RunBehaviour;
            pulseToTheBeat.enabled = false;
        }
    }
}
