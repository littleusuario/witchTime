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
    float gravityScale = 3f;
    bool jumping;
    public bool Jumping => jumping;
    public State_Jump(RandomMovement randomMovement, PlayerStateManager stateManager)
    {
        this.randomMovement = randomMovement;
        groundCheck = randomMovement.GroundCheck;
        this.stateManager = stateManager;
    }

    public void EnterState() 
    {
        if (!jumping) 
        {
            jumping = true;
            jumpVelocity = Mathf.Sqrt(impulse * -2 * (Physics.gravity.y * gravityScale));
        }
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
                jumping = false;
                maxHeightReached = false;
                stateManager.ChangeState(stateManager.state_Walking);
            }
        }

        bool moving = (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D));

        if (moving) 
        {
            stateManager.ChangeState(stateManager.state_Walking);
        }

        if (Input.GetMouseButtonDown(0))
        {
            stateManager.ChangeState(stateManager.state_Attacking);
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
        jumpVelocity += Physics.gravity.y * gravityScale * Time.deltaTime;

        randomMovement.transform.Translate(new Vector3(0, jumpVelocity, 0) * Time.deltaTime);

        if(randomMovement.transform.position.y < 0) 
        {
            randomMovement.transform.position = new Vector3(randomMovement.transform.position.x, 0, randomMovement.transform.position.z);
        }
    }
}
