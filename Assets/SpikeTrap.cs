using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : TrapObject
{
    [SerializeField] private float timeToToggle = 0.5f;
    [SerializeField] Animator animator;
    [SerializeField] float thressholdDistance = 1.0f;

    [SerializeField] private float activationTime = 0;
    [SerializeField] private float deactivationTime = 0;
    [SerializeField] bool trapOn = false;
    [SerializeField] Vector3 sizeOfTheBox = Vector3.zero;
    private void Start()
    {
        animator = GetComponent<Animator>();
        Initialize();
        transform.position = new Vector3(transform.position.x, 0 - 0.1f, transform.position.z);
    }
    void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < thressholdDistance) 
        {
            deactivationTime = 0;
            activationTime += Time.deltaTime;
            if (activationTime >= timeToToggle && !trapOn) 
            {
                trapOn = true;
                animator.SetBool("Activate", trapOn);
                activationTime = 0;
            }
        }
        else if (Vector3.Distance(transform.position, player.transform.position) >= thressholdDistance) 
        {
            activationTime = 0;
            deactivationTime += Time.deltaTime;
            if(deactivationTime >= timeToToggle) 
            {
                trapOn = false;
                animator.SetBool("Activate", trapOn);
                deactivationTime = 0;
            }    
        }

        if (trapOn) 
        {
            TrapActivate();
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, sizeOfTheBox);
        Gizmos.DrawWireSphere(transform.position, thressholdDistance);
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
