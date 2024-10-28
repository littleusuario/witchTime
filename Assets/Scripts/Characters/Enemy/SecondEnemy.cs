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
        RaycastHit[] hits;
        hits = Physics.SphereCastAll(transform.position, AttackRadius, transform.forward);

        foreach(RaycastHit hit in hits) 
        {
            GameObject hitObject = hit.collider.transform.gameObject;

            if (hitObject.CompareTag("Player"))
            {
                hitObject.GetComponent<PlayerHealth>().TakeDamage(1);
            }
        }
    }

    public override void TakeDamage()
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
