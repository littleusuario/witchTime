using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BeatSpikeTrap : TrapObject
{
    [SerializeField] Animator animator;

    private float activationTime = 0;
    private float deactivationTime = 0;
    [SerializeField] bool trapOn = false;
    [SerializeField] Vector3 sizeOfTheBox = Vector3.zero;
    private bool spikesOn = false;

    [SerializeField] private AudioClip activationSound;
    [SerializeField] private AudioClip deactivationSound;
    private AudioSource audioSource;

    [SerializeField] private float deactivationCooldown = 0.15f;
    BeatManager beatManager;
    private float deactivationCooldownTimer;

    private void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        Initialize();
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);

        GameObject gameObject = GameObject.FindGameObjectWithTag("BeatManager");
        beatManager = gameObject.GetComponent<BeatManager>();
        beatManager.Intervals[0]._Trigger.AddListener(ToggleTraps);
    }

    public void ToggleTraps() 
    {
        trapOn = !trapOn;
    }
    void Update()
    {
        if (originRoom == GameManager.Instance.ActualRoom)
        {
            ActivateTrap();
        }

        if (deactivationCooldownTimer > 0)
        {
            deactivationCooldownTimer -= Time.deltaTime;
            TrapActivate();
        }

        if (spikesOn)
        {
            TrapActivate();
        }
    }

    private void ActivateTrap()
    {
        if (trapOn)
        {
            activationTime += Time.deltaTime;
            if (activationTime >= 0.5f)
            {
                animator.SetBool("Activate", trapOn);
                spikesOn = true;

                PlaySound(activationSound);
                activationTime = 0;
                deactivationTime = 0;
            }
        }
        else
        {
            deactivationTime += Time.deltaTime;
            if (deactivationTime >= 0.5f)
            {
                spikesOn = false;
                animator.SetBool("Activate", trapOn);
                deactivationTime = 0;
                activationTime = 0;
                PlaySound(deactivationSound);

                deactivationCooldownTimer = deactivationCooldown;
            }
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, sizeOfTheBox);
    }

    public override void TrapActivate()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position, sizeOfTheBox / 2);

        foreach (Collider collider in colliders)
        {
            if (collider.transform.name == player.name)
                collider.gameObject.GetComponent<IDamageable>().TakeDamage(1);
        }
    }
}
