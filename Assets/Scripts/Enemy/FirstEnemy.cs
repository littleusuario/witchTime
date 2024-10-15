using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class FirstEnemy : Enemy
{
    GameObject player;
    private GameObject enemy;
    [SerializeField] private float velocity;
    [SerializeField] private float StepDistance = 0.5f;
    [SerializeField] CollisionManager hitboxEnemy;
    [SerializeField] float invincibilityFrames = 0.5f;
    [SerializeField] Animator animator;
    [SerializeField] PulseToTheBeat pulseToTheBeat;
    bool death;

    private void Awake()
    {
        if (hitboxEnemy != null)
        {
            hitboxEnemy.CollisionTrigger += TakeDamage;
        }

        if (pulseToTheBeat != null) 
        {
            //pulseToTheBeat.beatPulse += RunBehaviour;
        }
    }
    private void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Noytasar");
        if (playerObject != null) 
        {
            player = playerObject;
        }

        enemy = gameObject.transform.parent.gameObject;
    }

    private void Update()
    {
        enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, player.transform.position, StepDistance);
    }
    public override void Attack()
    {
        Debug.Log("El enemigo 1 ataca");
    }

    private void TakeDamage()
    {
        HealthPoints--;
        if (HealthPoints > 0)
        {
            if (animator != null) 
            {
                animator.SetBool("HitDamage", true);
                StartCoroutine(HitKnockBack());
                StartCoroutine(InvincibilityFrames());            
            }
        }
        else 
        {
            death = true;
            animator.SetBool("Death", true);
            StartCoroutine(HitKnockBack());
            StartCoroutine(InvincibilityFrames());
        }
    }

    IEnumerator InvincibilityFrames() 
    {
        hitboxEnemy.enabled = false;
        yield return new WaitForSeconds(invincibilityFrames);
        if (death) 
        {
            Destroy(gameObject);
            StopCoroutine("InvincibilityFrames");
            yield return null;
        }
        animator.SetBool("HitDamage", false);
        hitboxEnemy.enabled = true;
        yield return null;
    }

    IEnumerator HitKnockBack() 
    {
        float elapsedTime = 0;
        Vector3 initialPosition = enemy.transform.position;
        Vector3 finalPosition = enemy.transform.position - player.transform.position;

        while (elapsedTime < invincibilityFrames) 
        {
            elapsedTime += Time.deltaTime;
            enemy.transform.position = Vector3.Lerp(initialPosition, finalPosition.normalized, elapsedTime / invincibilityFrames);
            yield return null;
        }
        enemy.transform.position = finalPosition.normalized;
    }
}
