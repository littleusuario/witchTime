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
    private bool spikesOn = false;
    private bool steppedOnTrap = false;
    private float distance = 0;
    private void Start()
    {
        animator = GetComponent<Animator>();
        Initialize();
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
    }
    void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < thressholdDistance)
            steppedOnTrap = true;


        if (steppedOnTrap)
        {
            if (!trapOn) 
            {
                deactivationTime = 0;
                activationTime += Time.deltaTime;
                if (activationTime >= timeToToggle && !trapOn)
                {
                    trapOn = true;
                    animator.SetBool("Activate", trapOn);
                    activationTime = 0;
                    spikesOn = true;
                }
            }
            else
            {
                activationTime = 0;
                deactivationTime += Time.deltaTime;
                if (deactivationTime >= timeToToggle + timeToToggle/2 && trapOn)
                {
                    trapOn = false;
                    spikesOn = false;
                    animator.SetBool("Activate", trapOn);
                    deactivationTime = 0;
                    steppedOnTrap = false;
                }
            }
        }

        if (spikesOn) 
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
