using UnityEngine;

public class SlashAttack : MonoBehaviour
{
    [SerializeField] float sphereSize = 1.75f;
    [SerializeField] int weaponDamage = 1;
    GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    void Update()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, sphereSize);

        if (hitColliders == null) return;
        foreach (Collider collider in hitColliders)
        {
            IDamageable damageable = collider.gameObject.GetComponent<IDamageable>();

            if (damageable != null && collider.gameObject != player)
            {
                damageable.TakeDamage(weaponDamage);
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, sphereSize);
    }
}

