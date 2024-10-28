using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Attack : IState
{
    RandomMovement randomMovement;
    PlayerStateManager stateManager;
    bool flipped;
    float thresholdAngle = 5f;

    public State_Attack(RandomMovement randomMovement, PlayerStateManager stateManager)
    {
        this.randomMovement = randomMovement;
        this.stateManager = stateManager;
    }

    public void EnterState()
    {
        randomMovement.animator.SetBool("up", false);
        randomMovement.animator.SetBool("down", false);
        randomMovement.animator.SetBool("left", false);
        randomMovement.animator.SetBool("right", false);

        randomMovement.animator.SetBool("attacking", true);
    }

    public void UpdateState()
    {
        Vector3 mousePOS = Camera.main.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * 10f);
        float angle = AngleBetweenTwoPoints(randomMovement.transform.position, mousePOS);
        UpdateParticleSystem(angle, mousePOS);

        if (randomMovement.SlashParticle.activeSelf)
        {
            UpdateMouseBools(mousePOS);
            MoveTowardsMouse(mousePOS);
        }
        else
        {
            flipped = false;
        }

        CheckForStateChange();
    }

    private void UpdateParticleSystem(float angle, Vector3 mousePOS)
    {
        var particleSystemMain = randomMovement.SlashParticle;
        particleSystemMain.transform.eulerAngles = new Vector3(0, -angle + 90, 0);

        if (!flipped)
        {
            flipped = true;
            FlipParticleSystem(particleSystemMain);
        }
    }

    private void MoveTowardsMouse(Vector3 mousePOS)
    {
        Vector3 newPosition = Vector3.MoveTowards(randomMovement.transform.position, mousePOS, randomMovement.velocidad * Time.deltaTime);
        newPosition.y = 0;

        if (IsOnGround(newPosition) && !CheckCollisions(newPosition))
        {
            randomMovement.transform.position = newPosition;
        }
    }

    private bool IsOnGround(Vector3 position)
    {
        RaycastHit hit;
        if (Physics.Raycast(position + Vector3.up, Vector3.down, out hit, 2f))
        {
            return hit.collider != null && hit.collider.CompareTag("Ground");
        }
        return false;
    }

    private void FlipParticleSystem(GameObject particleSystemMain)
    {
        var main = particleSystemMain.GetComponent<ParticleSystem>().main;
        main.startRotationY = main.startRotationY.constantMax * -1;
        main.startRotationZ = main.startRotationZ.constantMax * -1;
        main.startRotationX = main.startRotationX.constantMax * -1;
    }

    private bool CheckCollisions(Vector3 newPosition)
    {
        List<Ray> rayList = randomMovement.CollisionHitbox.StoredRays;
        var positionAndNewPos = (newPosition - randomMovement.transform.position).normalized;
        bool isColliding = false;

        foreach (var ray in rayList)
        {
            var positionColliding = (randomMovement.transform.position - ray.direction).normalized;
            if (randomMovement.CollisionHitbox.CollisionBools[rayList.IndexOf(ray)] &&
                Vector3.Dot(positionAndNewPos, positionColliding) >= -0.5f)
            {
                isColliding = true;
            }
        }
        return isColliding;
    }

    private void CheckForStateChange()
    {
        if (!Input.GetMouseButton(0) && !randomMovement.PulseOfTheBeat.PulseInput)
        {
            if (!stateManager.state_Jumping.Jumping)
            {
                stateManager.ChangeState(stateManager.state_Walking);
            }
            else
            {
                stateManager.ChangeState(stateManager.state_Jumping);
            }
        }
    }

    private void UpdateMouseBools(Vector3 mousePOS)
    {
        Vector3 direction = mousePOS - randomMovement.transform.position;
        ResetAnimationBools();

        bool isHorizontal = Mathf.Abs(direction.x) > 0.5f * Mathf.Abs(direction.z);
        bool isVertical = Mathf.Abs(direction.z) > 0.5f * Mathf.Abs(direction.x);

        if (isHorizontal && !isVertical)
        {
            randomMovement.animator.SetBool(direction.x < 0 ? "mouseLeft" : "mouseRight", true);
        }
        else if (isVertical && !isHorizontal)
        {
            randomMovement.animator.SetBool(direction.z > 0 ? "mouseUp" : "mouseDown", true);
        }
        else
        {
            randomMovement.animator.SetBool(direction.x < 0 ? "mouseLeft" : "mouseRight", true);
            randomMovement.animator.SetBool(direction.z > 0 ? "mouseUp" : "mouseDown", true);
        }

        randomMovement.StartCoroutine(ResetMouseBools());
    }

    private IEnumerator ResetMouseBools()
    {
        yield return new WaitForSeconds(0.2f);
        ResetAnimationBools();
    }

    private void ResetAnimationBools()
    {
        randomMovement.animator.SetBool("mouseLeft", false);
        randomMovement.animator.SetBool("mouseRight", false);
        randomMovement.animator.SetBool("mouseUp", false);
        randomMovement.animator.SetBool("mouseDown", false);
    }

    public void ExitState()
    {
        ResetAnimationBools();
        randomMovement.animator.SetBool("attacking", false);
    }

    private float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }
}