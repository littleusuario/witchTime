using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public Transform referenceObject;
    public List<Vector3> localPositions = new List<Vector3>();
    public float speed = 1.0f;
    public float smoothTime = 0.3f;

    public int currentTargetIndex = 1;
    private Vector3 velocity = Vector3.zero;
    private bool isMoving = false;

    public GameObject leftObject;
    public GameObject rightObject;

    void Start()
    {
        if (referenceObject != null)
        {
            transform.position = referenceObject.TransformPoint(localPositions[1]);
        }

        UpdateObjectStates();
    }

    void Update()
    {
        if (isMoving)
        {
            Vector3 targetPosition = referenceObject.TransformPoint(localPositions[currentTargetIndex]);
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime, speed);

            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                transform.position = targetPosition;
                isMoving = false;
                UpdateObjectStates();
            }
        }
    }

    public void GoNext()
    {
        if (currentTargetIndex < localPositions.Count - 1 && !isMoving)
        {
            StartMovement(currentTargetIndex + 1);
        }
    }

    public void GoBack()
    {
        if (currentTargetIndex > 0 && !isMoving)
        {
            StartMovement(currentTargetIndex - 1);
        }
    }

    private void StartMovement(int targetIndex)
    {
        isMoving = true;
        currentTargetIndex = targetIndex;

        leftObject.SetActive(false);
        rightObject.SetActive(false);
    }

    private void UpdateObjectStates()
    {
        if (currentTargetIndex == 0)
        {
            leftObject.SetActive(false);
            rightObject.SetActive(true);
        }
        else if (currentTargetIndex == 1)
        {
            leftObject.SetActive(true);
            rightObject.SetActive(true);
        }
        else if (currentTargetIndex == 2)
        {
            leftObject.SetActive(true);
            rightObject.SetActive(false);
        }
    }
}
