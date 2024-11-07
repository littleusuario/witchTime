using System.Collections.Generic;
using UnityEngine;

public class RandomMovement : MonoBehaviour
{
    [Header("Variables de movimiento")]
    [SerializeField] Vector3[] particleRotations;
    [SerializeField] GameObject slashParticle;
    [SerializeField] CollisionController collisionHitbox;
    [SerializeField] CollisionController groundCheck;
    [SerializeField] private PulseToTheBeat pulseOfTheBeat;

    [Header("Sonidos")]
    [SerializeField] private List<AudioClip> sounds;
    [SerializeField] AudioSource attackAudioSource;

    private Camera mainCamera;

    public float velocidad = 2f;
    public float maxJumpHeight = 0.01f;
    public Animator animator;
    public GameObject SlashParticle => slashParticle;
    public Vector3[] ParticleRotations => particleRotations;
    public CollisionController CollisionHitbox => collisionHitbox;
    public CollisionController GroundCheck => groundCheck;
    public PlayerStateManager stateManager;
    public PulseToTheBeat PulseOfTheBeat => pulseOfTheBeat;

    void Awake()
    {
        stateManager = new PlayerStateManager(this);
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera")?.GetComponent<Camera>();
    }

    void Update()
    {
        stateManager.Update();
    }

    public void PlayRandomSound()
    {
        if (sounds.Count > 0)
        {
            AudioClip randomSound = sounds[Random.Range(0, sounds.Count)];
            attackAudioSource.PlayOneShot(randomSound);
        }
    }
}
