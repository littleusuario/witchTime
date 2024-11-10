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
    [SerializeField] AudioSource landSound;

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

    private bool canMove = true;
    private float movementCooldown = 0f;
    private bool canProcessInput = false;

    void Awake()
    {
        stateManager = new PlayerStateManager(this);
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera")?.GetComponent<Camera>();
        WaitforLand(0.6f);
    }

    void Update()
    {
        if (movementCooldown > 0f)
        {
            movementCooldown -= Time.deltaTime;
            if (movementCooldown <= 0f)
            {
                canMove = true;
                canProcessInput = true;
                GameManager.Instance.PauseGameForSeconds(0.09f);
                landSound.Play();
                GameManager.Instance.TriggerCameraShake(1f, 3f, 0.1f);
            }
        }

        stateManager.Update();
    }

    public void WaitforLand(float seconds)
    {
        canMove = true;
        canProcessInput = false;
        movementCooldown = seconds;
    }

    public bool CanMove()
    {
        return canMove;
    }

    public bool CanProcessInput()
    {
        return canProcessInput;
    }

    public void PlayRandomSound()
    {
        if (sounds.Count > 0)
        {
            AudioClip randomSound = sounds[Random.Range(0, sounds.Count)];
            attackAudioSource.PlayOneShot(randomSound);
        }
    }

    public void SwitchStateDeath()
    {
        stateManager.ChangeState(stateManager.state_PlayerDeath);
    }
}
