using System.Collections;
using System.Collections.Generic;
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
    bool death;

    private void Awake()
    {
        if (hitboxEnemy != null)
        {
            hitboxEnemy.CollisionTrigger += TakeDamage;
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
    public override void Attack()
    {
        Debug.Log("El enemigo 1 ataca");
    }

    private void Update()
    {
        enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, player.transform.position, StepDistance);
    }

    private void TakeDamage()
    {
        HealthPoints--;
        if (HealthPoints > 0)
        {
            if (animator == null) 
            {
                animator.SetBool("HitDamage", true);
                StartCoroutine(InvicibilityFrames());            
            }
        }
        else 
        {
            death = true;
            animator.SetBool("Death", true);
            StartCoroutine(InvicibilityFrames());
        }
    }

    IEnumerator InvicibilityFrames() 
    {
        hitboxEnemy.enabled = false;
        yield return new WaitForSeconds(invincibilityFrames);
        if (death) 
        {
            Destroy(gameObject);
        }
        animator.SetBool("HitDamage", false);
            hitboxEnemy.enabled = true;
    }
}
