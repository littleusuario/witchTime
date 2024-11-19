using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Attack : IState
{
    RandomMovement randomMovement;
    PlayerStateManager stateManager;
    bool flipped;
    float thresholdAngle = 5f;
    private bool hasAttacked;
    private GameObject cursorObject;

    public State_Attack(RandomMovement randomMovement, PlayerStateManager stateManager)
    {
        this.randomMovement = randomMovement;
        this.stateManager = stateManager;
    }

    public void EnterState()
    {
        cursorObject = GameObject.FindWithTag("Cursor");

        randomMovement.animator.SetFloat("Horizontal", 0f);
        randomMovement.animator.SetFloat("Vertical", 0f);

        randomMovement.animator.SetBool("attacking", true);
        hasAttacked = false;
    }

    public void UpdateState()
    {
        if (GameManager.Instance.IsGamePaused)
        {
            return;
        }

        if (cursorObject == null) return;

        Vector3 cursorPos = cursorObject.transform.position;
        float angle = AngleBetweenTwoPoints(randomMovement.transform.position, cursorPos);
        UpdateParticleSystem(angle, cursorPos);

        if (randomMovement.SlashParticle.activeSelf)
        {
            UpdateMouseBools(cursorPos);
            MoveTowardsCursor(cursorPos);

            if (!hasAttacked)
            {
                randomMovement.PlayRandomSound();
                hasAttacked = true;
            }
        }
        else
        {
            flipped = false;
            hasAttacked = false;
        }

        CheckForStateChange();
    }

    private void UpdateParticleSystem(float angle, Vector3 cursorPos)
    {
        var particleSystemMain = randomMovement.SlashParticle;
        particleSystemMain.transform.eulerAngles = new Vector3(0, -angle + 90, 0);

        if (!flipped)
        {
            flipped = true;
            FlipParticleSystem(particleSystemMain);
        }
    }

    private void MoveTowardsCursor(Vector3 cursorPos)
    {
        Vector3 newPosition = Vector3.MoveTowards(randomMovement.transform.position, cursorPos, randomMovement.velocidad * Time.deltaTime);
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
            stateManager.ChangeState(stateManager.state_Walking);
        }
    }

    private void UpdateMouseBools(Vector3 cursorPos)
    {
        Vector3 direction = (cursorPos - randomMovement.transform.position).normalized;

        randomMovement.animator.SetFloat("MouseHorizontal", direction.x);
        randomMovement.animator.SetFloat("MouseVertical", direction.z);

        randomMovement.StartCoroutine(ResetMouseBools());
    }

    private IEnumerator ResetMouseBools()
    {
        yield return new WaitForSeconds(0.2f);
        ResetAnimationBools();
    }

    private void ResetAnimationBools()
    {
        randomMovement.animator.SetFloat("MouseHorizontal", 0);
        randomMovement.animator.SetFloat("MouseVertical", 0);
    }

    public void ExitState()
    {
        ResetAnimationBools();
        randomMovement.animator.SetBool("attacking", false);
        hasAttacked = false;
    }

    private float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }
}