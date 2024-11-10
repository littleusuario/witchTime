using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoorLevel : MonoBehaviour
{
    Animator animator;
    float elapsedTime = 0;
    bool doTransitionOnce;
    GameObject player;

    public float TimeToStartTranstion = 1.3f;
    public GameObject CameraFollowObject;
    private void Start()
    {
        animator = GetComponent<Animator>();
        CameraFollowObject = GameObject.FindGameObjectWithTag("ObjectFollowCam");
        player = GameObject.FindGameObjectWithTag("Player");
    }
    public void InitializeTransition() 
    {
        animator.SetBool("Closing", true);
        player.SetActive(false);
        doTransitionOnce = true;
        elapsedTime += Time.deltaTime;
    }


    void Update()
    {
        if (doTransitionOnce)
        {
            if(elapsedTime >= TimeToStartTranstion) 
            {
                StartCoroutine(MoveCamera());
                doTransitionOnce = false;
            }
            else
                elapsedTime += Time.deltaTime;
        }
    }

    IEnumerator MoveCamera() 
    {
        float elapsedTime = 0;
        Vector3 initialPosition = CameraFollowObject.transform.position;
        Vector3 endPosition = new Vector3(initialPosition.x, -15, initialPosition.z);

        while (elapsedTime < TimeToStartTranstion) 
        {
            elapsedTime += Time.deltaTime;
            CameraFollowObject.transform.position = Vector3.Lerp(initialPosition, endPosition, elapsedTime / TimeToStartTranstion);
            yield return null;
        }

        CameraFollowObject.transform.position = endPosition;
        GameManager.Instance.LoadNextLevel();
    }
}
    

