using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    [SerializeField] float rayLimit = 0.2f;
    [SerializeField] List<Ray> storedRays = new List<Ray>();
    [SerializeField] List<string> tagsToCollide = new List<string>();
    [SerializeField] Vector3[] directions;
    [SerializeField] bool rayHit;
    [SerializeField] string tagCompare = string.Empty;
    public event Action CollisionTrigger;

    public List<Ray>StoredRays => storedRays;
    public bool RayHit => rayHit;
    public float RayLimit => rayLimit;
    void Update()
    {
        for (int i = 0; i < directions.Length; i++) 
        {
            if (storedRays.Count < directions.Length) 
            {
                storedRays.Add(new Ray(transform.position, directions[i]));
            }
            storedRays[i] = new Ray(transform.position, directions[i]);
        }

        foreach (Ray ray in storedRays) 
        {
            RaycastHit hit;
            bool currentRayHit = false;
            Vector3 rayDir = new Vector3(Mathf.Clamp(ray.direction.x, -rayLimit, rayLimit), Mathf.Clamp(ray.direction.y, -rayLimit, rayLimit), Mathf.Clamp(ray.direction.z, -rayLimit, rayLimit));
            Debug.DrawRay(ray.origin, rayDir, Color.green);
            if (Physics.Raycast(ray, out hit, rayLimit)) 
            {
                Collider collider = hit.collider;
                MeshRenderer meshRenderer = null;
                foreach (string tag in tagsToCollide) 
                {
                    if (hit.transform.gameObject.CompareTag(tag)) 
                    {
                        Debug.DrawRay(ray.origin, rayDir, Color.red);
                        rayHit = true;
                        currentRayHit = true;
                        if (CollisionTrigger != null) 
                        {
                            CollisionTrigger.Invoke();
                            break;
                        }
                    }
                }
            }

            if (hit.collider == null && rayHit && !currentRayHit)
            {
                rayHit = false;
            }
        }

        //storedRays.Clear();
    }
}