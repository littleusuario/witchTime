using System;
using UnityEngine;

public class RandomMovement : MonoBehaviour
{
    [Header("Variables de movimiento")]
    public float velocidad = 2f;

    public Animator animator;
    private Camera mainCamera;
    [SerializeField] Vector3[] particleRotations;
    [SerializeField] GameObject slashParticle;
    public GameObject SlashParticle => slashParticle;
    public Vector3[] ParticleRotations => particleRotations;

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
