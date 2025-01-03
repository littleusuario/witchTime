using System.Collections;
using UnityEngine;

public class SpinObject : MonoBehaviour
{
    [SerializeField] Vector3[] rotations;
    [SerializeField] float timeToRotate = 3f;
    [SerializeField] int boxesMenu = 0;
    bool Rotating;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) && !Rotating && boxesMenu > 0) 
        {
            boxesMenu--;
            StartCoroutine(SpinRotation(rotations[boxesMenu]));
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) && !Rotating && boxesMenu < 3)
        {
            boxesMenu++;
            StartCoroutine(SpinRotation(rotations[boxesMenu]));
        }
    }

    public void SpinToOptions(int RoomToRotate)
    {
        if (!Rotating) 
        {
            StartCoroutine(SpinRotation(rotations[RoomToRotate]));
        }
    }

    public void SpinToCredits(int RoomToRotate)
    {
        if (!Rotating)
        {
            StartCoroutine(SpinRotation(rotations[RoomToRotate]));
        }
    }

    public void SpinToMenu(int RoomToRotate)
    {
        if (!Rotating)
        {
            StartCoroutine(SpinRotation(rotations[RoomToRotate]));
        }
    }

    IEnumerator SpinRotation(Vector3 endEulerAngles) 
    {
        float elapsedTime = 0;
        Vector3 initialEulerAngles = transform.eulerAngles;
        Rotating = true;
        while (elapsedTime < timeToRotate) 
        {
            elapsedTime += Time.deltaTime;
            transform.eulerAngles = Vector3.Slerp(initialEulerAngles, endEulerAngles, elapsedTime / timeToRotate);
            yield return null;
        }

        transform.eulerAngles = endEulerAngles;
        Rotating = false;
    }
}
