using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashAttack : MonoBehaviour
{
    [SerializeField] Vector3 hitboxHalfSize;
    [SerializeField] Vector3 offset;
    [SerializeField] Vector3 rotation;
    void Update()
    {
        rotation = transform.rotation.eulerAngles;
        RaycastHit[] hits;
        hits = Physics.BoxCastAll(transform.position + offset, hitboxHalfSize, Vector3.forward, transform.rotation);

        foreach (RaycastHit hit in hits)
        {
            Enemy hitObject = hit.collider.transform.gameObject.GetComponent<Enemy>();

            if (hitObject != null)
            {
                Debug.Log("HELP");
                hitObject.TakeDamage();
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position + offset, hitboxHalfSize * 2);
    }
}
