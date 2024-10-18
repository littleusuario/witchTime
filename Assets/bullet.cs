using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    private float vel = 2;
    [SerializeField] Rigidbody rb;
    private float angle = 0;
    public float Angle {  get { return angle; } set {  angle = value; } }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.AddForce(vel * Time.fixedDeltaTime, 0, 0);
    }

    public class bulletPulse : MonoBehaviour
    {

    }
}
