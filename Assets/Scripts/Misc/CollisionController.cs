using System;
using System.Collections.Generic;
using UnityEngine;

public class CollisionController : MonoBehaviour
{
    [SerializeField] float rayLimit = 0.2f;
    [SerializeField] List<Ray> storedRays = new List<Ray>();
    [SerializeField] List<string> tagsToCollide = new List<string>();
    [SerializeField] Vector3[] directions;
    [SerializeField] bool rayHit;
    [SerializeField] string tagCompare = string.Empty;
    [SerializeField] private bool[] collisionBools = new bool[4];
    
    bool blockForward;
    bool blockBack;
    bool blockRight;
    bool blockLeft;

    public event Action CollisionTrigger;
    public List<Ray>StoredRays => storedRays;
    public bool RayHit => rayHit;
    public float RayLimit => rayLimit;
    public bool[] CollisionBools => collisionBools;


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

        if (storedRays.Count >= 3) 
        {
            collisionBools[0] = blockForward = CheckRayHit(0);
            collisionBools[1] = blockBack = CheckRayHit(1);
            collisionBools[2] = blockRight = CheckRayHit(2);
            collisionBools[3] = blockLeft = CheckRayHit(3);
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
    }

    bool CheckRayHit(int rayIndex)
    {
        if (Physics.Raycast(storedRays[rayIndex], out RaycastHit directionHit, rayLimit)) 
        {
            bool rayHit = false;
            foreach (string tag in tagsToCollide) 
            {
                if (directionHit.transform.gameObject.CompareTag(tag)) 
                {
                    rayHit = true;
                }

                if (rayHit && directionHit.transform.gameObject.CompareTag("Exit")) 
                {
                    GameManager.Instance.LoadNextLevel();
                }
            }
            return rayHit;
        }
        else 
        {
            return false;
        }
    }
}
