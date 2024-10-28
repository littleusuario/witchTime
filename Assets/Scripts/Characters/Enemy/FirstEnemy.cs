using System;
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
    [SerializeField] CollisionController hitboxEnemy;
    [SerializeField] float invincibilityFrames = 0.5f;
    [SerializeField] Animator animator;
    [SerializeField] PulseToTheBeat pulseToTheBeat;
    bool death;
    public override event Action Ondie;
    EnemyStateManager stateManager;

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
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null) 
        {
            player = playerObject;
        }

        enemy = gameObject.transform.parent.gameObject;

        State_SkeletonNormal state_SkeletonNormal = new State_SkeletonNormal(this);
        State_SkeletonDeath state_SkeletonDeath = new State_SkeletonDeath(this);
        stateManager = new EnemyStateManager(state_SkeletonNormal, state_SkeletonDeath);

    }

    public override void RunBehaviour()
    {
        stateManager.UpdateState();
    }

    private void TakeDamage()
    {
        HealthPoints--;
        if (HealthPoints > 0)
        {
            if (animator != null) 
            {
                StartCoroutine(InvincibilityFrames());            
            }
        }
        else 
        {
            death = true;
            animator.SetBool("Death", true);
            StartCoroutine(InvincibilityFrames());
        }
        if (HealthPoints == 0)
        {
            Ondie.Invoke();
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

    private void OnDestroy()
    {
        pulseToTheBeat.beatPulse -= RunBehaviour;
        pulseToTheBeat.enabled = false;
    }
}
