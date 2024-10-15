using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Jump : IState
{
    RandomMovement randomMovement;
    PlayerStateManager stateManager;
    private bool maxHeightReached;
    CollisionManager groundCheck;
    float impulse = 1f;
    float limitHeight = 0.5f;
    float jumpVelocity = 5f;

    public State_Jump(RandomMovement randomMovement, PlayerStateManager stateManager)
    {
        this.randomMovement = randomMovement;
        groundCheck = randomMovement.GroundCheck;
        this.stateManager = stateManager;
    }

    public void EnterState() 
    {
        jumpVelocity = Mathf.Sqrt(impulse * -2 * Physics.gravity.y);
    }

    public void UpdateState() 
    {
        if (!maxHeightReached) 
        {
            Jump();
        }

        if (maxHeightReached && groundCheck != null) 
        {
            SimulatePhysics();
            if (groundCheck.RayHit) 
            {
                maxHeightReached = false;
                stateManager.ChangeState(stateManager.state_Walking);
            }
        }
    }
    public void ExitState() 
    {
    
    }
    public void Jump() 
    {
        Debug.Log(jumpVelocity);
        SimulatePhysics();

        if (randomMovement.transform.position.y >= limitHeight) 
        {
            maxHeightReached = true;
            //Debug.Log("Max heaight reached");
        }
    }

    private void SimulatePhysics() 
    {
        jumpVelocity += Physics.gravity.y * Time.deltaTime;

        randomMovement.transform.Translate(new Vector3(0, jumpVelocity, 0) * Time.deltaTime);

        if(randomMovement.transform.position.y < 0) 
        {
            randomMovement.transform.position = new Vector3(randomMovement.transform.position.x, 0, randomMovement.transform.position.z);
        }
    }
}
