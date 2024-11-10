using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartPickUp : MonoBehaviour
{
    [SerializeField] private int healAmount = 1;
    [SerializeField] private float sphereRad;
    public float timeLapse;

    [SerializeField] private AudioClip pickupSound;
    private AudioSource audioSource;

    private Vector3 origin;
    private bool isPickedUp = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (isPickedUp) return;

        origin = transform.position;
        timeLapse += Time.deltaTime;
        if (timeLapse > 1)
        {
            Collider[] hitColliders = Physics.OverlapSphere(origin, sphereRad);
            foreach (var hit in hitColliders)
            {
                if (hit.transform.gameObject.CompareTag("Player"))
                {
                    PlayerHealth addHealth = hit.gameObject.GetComponent<PlayerHealth>();
                    audioSource.PlayOneShot(pickupSound);

                    addHealth.Heal(healAmount);
                    isPickedUp = true;

                    StartCoroutine(DestroyAfterDelay());
                }
            }
        }
    }

    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(this.gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, sphereRad);
    }
}
