using System;
using UnityEngine;

public class RandomMovement : MonoBehaviour
{
    [Header("Variables de movimiento")]
    public float velocidad = 2f;
    public float maxJumpHeight = 0.01f;

    public Animator animator;
    private Camera mainCamera;
    [SerializeField] Vector3[] particleRotations;
    [SerializeField] GameObject slashParticle;
    [SerializeField] CollisionManager collisionHitbox;
    [SerializeField] CollisionManager groundCheck;
    public GameObject SlashParticle => slashParticle;
    public Vector3[] ParticleRotations => particleRotations;
    public CollisionManager CollisionHitbox => collisionHitbox;
    public CollisionManager GroundCheck => groundCheck;

    public StateManager stateManager;
    void Awake()
    {
        stateManager = new StateManager(this);
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera")?.GetComponent<Camera>();
    }

    void Update()
    {
        stateManager.Update();
    }
}
