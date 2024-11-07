using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    [SerializeField] private BulletScriptable bulletData;
    [SerializeField] private float sphereRad;
    
    ObjectPool<Bullet> pool;
    private Vector3 origin;
    private Vector3 direction;
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
            IDamageable damageable = hit.gameObject.GetComponent<IDamageable>();

            if (damageable != null)
            {
                Pool.Release(this);
                damageable.TakeDamage(bulletData.damage);
                return;
            }
            else 
            {            
                Pool.Release(this);
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
