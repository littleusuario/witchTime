using UnityEngine;

public class SlashAttack : MonoBehaviour
{
    [SerializeField] Vector3 hitboxHalfSize;
    [SerializeField] Vector3 offset;
    [SerializeField] Vector3 rotation;
    [SerializeField] int weaponDamage = 1;
    GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    void Update()
    {
        rotation = transform.rotation.eulerAngles;
        RaycastHit[] hits;
        hits = Physics.BoxCastAll(transform.position + offset, hitboxHalfSize, Vector3.forward, transform.rotation);

        foreach (RaycastHit hit in hits)
        {
            IDamageable damageable = hit.collider.gameObject.GetComponent<IDamageable>();

            if (damageable != null && hit.collider.gameObject != player)
            {
                damageable.TakeDamage(weaponDamage);
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position + offset, hitboxHalfSize * 2);
    }
}
