using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    ObjectPool<Bullet> pool;
    [SerializeField] private float sphereRad;
    private Vector3 origin;
    private Vector3 direction;
    [SerializeField] private BulletScriptable bulletData;
    public LayerMask mask;
    public float maxdist;

    public ObjectPool<Bullet> Pool { get { return pool; } set { pool = value; } }

    private void Update()
    {
        origin = transform.position;
        direction = transform.position + transform.forward * maxdist;

        Collider[] hitColliders = Physics.OverlapSphere(origin, sphereRad, mask);
        foreach (var hit in hitColliders)
        {
            if (hit.CompareTag("Wall"))
            {
                Pool.Release(this);
                return;
            }
            if (hit.CompareTag("Player"))
            {
                Pool.Release(this);
                hit.gameObject.GetComponent<PlayerHealth>().TakeDamage(bulletData.damage);
                return;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, sphereRad);
    }
}
