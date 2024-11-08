using System;
using System.Collections;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IDamageable
{
    [Header("Enemy Parent Class")]
    public int HealthPoints;
    public int AttackRadius = 5;
    public event Action Ondie;
    public ParticleSystem DamageParticleSystem;
    public float timeInactive = 1;
    public float elapsedTime = 0;
    protected GameObject player;
    protected GameObject enemy;
    protected Material material;
    protected float distance = 0;
    protected EnemyStateManager stateManager;
    [SerializeField] protected float thresholdAttackDistance = 1.5f;
    [SerializeField] protected Animator animator;
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] protected CollisionController hitboxEnemy;
    [SerializeField] protected PulseToTheBeat pulseToTheBeat;

    [ColorUsage(true, true)]
    [SerializeField] protected Color flashColor = Color.white;
    protected bool death;
    protected void InitializeEnemy(SpriteRenderer spriteRenderer) 
    {
        if (pulseToTheBeat != null)
        {
            pulseToTheBeat.beatPulse += RunBehaviour;
        }

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
            stateManager.UpdateState();
    }
    protected void DamageZone() 
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
    public void TakeDamage(int damage) 
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
        }

        if (HealthPoints == 0 && Ondie != null)
        {
            Ondie.Invoke();
        }
    }

    protected IEnumerator HitEffect()
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

    protected IEnumerator InvincibilityFrames()
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


    protected void OnDestroy()
    {
        if (pulseToTheBeat != null)
        {
            pulseToTheBeat.beatPulse -= RunBehaviour;
            pulseToTheBeat.enabled = false;
        }
    }
}
