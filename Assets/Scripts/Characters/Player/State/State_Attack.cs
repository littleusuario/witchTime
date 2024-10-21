using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Attack : IState
{
    RandomMovement randomMovement;
    PlayerStateManager stateManager;
    bool flipped;
    bool colliding;
    float thresholdAngle = 5f;
    public State_Attack(RandomMovement randomMovement, PlayerStateManager stateManager)
    {
        this.randomMovement = randomMovement;
        this.stateManager = stateManager;
    }

    public void EnterState()
    {

    }

    public void UpdateState()
    {
        Vector3 mousePOS = Camera.main.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * 10f);
        float angle = AngleBetweenTwoPoints(randomMovement.transform.position, mousePOS);
        var particleSystemMain = randomMovement.SlashParticle;
        ParticleSystemRenderer particleRenderer = particleSystemMain.GetComponent<ParticleSystemRenderer>();

        particleSystemMain.transform.eulerAngles = new Vector3(0, -angle + 90, 0);

        if (particleSystemMain.activeSelf)
        {
            Vector3 newPosition = randomMovement.transform.position;
            newPosition = Vector3.MoveTowards(randomMovement.transform.position, mousePOS, randomMovement.velocidad * Time.deltaTime);
            newPosition.y = 0;

            if (!flipped)
            {
                flipped = true;

                var main = particleSystemMain.GetComponent<ParticleSystem>().main;
                float rotationY = main.startRotationY.constantMax;
                main.startRotationY = rotationY * -1;
                float rotationZ = main.startRotationZ.constantMax;
                main.startRotationZ = rotationZ * -1;
                float rotationX = main.startRotationX.constantMax;
                main.startRotationX = rotationX * -1;
            }

            checkCollisions(newPosition);

        }
        //randomMovement.transform.Translate((mousePOS - randomMovement.transform.position).normalized * randomMovement.velocidad * Time.deltaTime);

        else
        {
            flipped = false;
        }

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

    public void ExitState() 
    {
        
    }

    float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }

    private void checkCollisions(Vector3 newPosition) 
    {
        List<Ray> rayList = randomMovement.CollisionHitbox.StoredRays;

        bool colliding = false;
        var positionAndNewPos = (newPosition - randomMovement.transform.position).normalized;

        for (int i = 0; i < rayList.Count; i++) 
        {
            var positionColliding = (randomMovement.transform.position - rayList[i].direction).normalized;
            float dot = Vector3.Dot(positionAndNewPos, positionColliding);
            if (randomMovement.CollisionHitbox.CollisionBools[i] && dot >= -0.5f)
            {
                colliding = true;
            }
            else 
            {

            }
        }

        if (!colliding) 
        {
            randomMovement.transform.position = newPosition;
        }
    }
}
