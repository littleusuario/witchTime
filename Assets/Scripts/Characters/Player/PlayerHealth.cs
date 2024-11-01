using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Cinemachine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private UnityEvent OnDie = new UnityEvent();
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip damageSound;

    [ColorUsage(true, true)]
    [SerializeField] private Color flashColor = Color.white;

    private Material material;
    private bool isInvulnerable = false;
    private float invulnerabilityDuration = 3f;
    private float blinkInterval = 0.1f;

    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin perlin;

    private float amplitude = 0.5f;
    private float frequency = 10f;

    [Header("Health Display")]
    [SerializeField] private List<GameObject> healthIcons;

    private void Start()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        if (spriteRenderer != null)
        {
            material = spriteRenderer.material;
        }

        if (virtualCamera != null)
        {
            perlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            if (perlin != null)
            {
                perlin.m_AmplitudeGain = 0f;
                perlin.m_FrequencyGain = 0f;
            }
        }

        UpdateHealthDisplay();
    }

    public void TakeDamage(int damage)
    {
        if (isInvulnerable) return;

        GameManager.Instance.TakePlayerDamage(damage);
        Debug.Log(GameManager.Instance.GetPlayerCurrentHealth());

        if (audioSource != null && damageSound != null)
        {
            audioSource.PlayOneShot(damageSound);
        }

        UpdateHealthDisplay();

        if (GameManager.Instance.GetPlayerCurrentHealth() <= 0)
        {
            OnDie.Invoke();
            GameManager.Instance.ResetPlayerHealth();
            GameManager.Instance.LoadNextLevel();
        }
        else
        {
            StartCoroutine(Invulnerability());
        }
    }


    private void UpdateHealthDisplay()
    {
        int currentHealth = GameManager.Instance.GetPlayerCurrentHealth();
        for (int i = 0; i < healthIcons.Count; i++)
        {
            healthIcons[i].SetActive(i < currentHealth);
        }
    }

    private IEnumerator Invulnerability()
    {
        isInvulnerable = true;

        float elapsedTime = 0f;
        float currentFlashAmount = 1f;

        material.SetColor("_FlashColor", flashColor);
        if (perlin != null)
        {
            perlin.m_AmplitudeGain = amplitude;
            perlin.m_FrequencyGain = frequency;
        }

        Color originalColor = spriteRenderer.color;
        spriteRenderer.color = Color.red;

        while (elapsedTime < invulnerabilityDuration)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;

            float lerpFactor = elapsedTime / invulnerabilityDuration;
            currentFlashAmount = Mathf.Lerp(1f, 0f, lerpFactor);
            material.SetFloat("_FlashAmount", currentFlashAmount);

            if (perlin != null)
            {
                perlin.m_AmplitudeGain = Mathf.Lerp(amplitude, 0f, lerpFactor);
                perlin.m_FrequencyGain = Mathf.Lerp(frequency, 0f, lerpFactor);
            }

            yield return new WaitForSeconds(blinkInterval);
            elapsedTime += blinkInterval;
        }

        spriteRenderer.enabled = true;
        spriteRenderer.color = originalColor;
        material.SetFloat("_FlashAmount", 0f);
        if (perlin != null)
        {
            perlin.m_AmplitudeGain = 0f;
            perlin.m_FrequencyGain = 0f;
        }

        isInvulnerable = false;
    }
}
