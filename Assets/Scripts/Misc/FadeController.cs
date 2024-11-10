using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeController : MonoBehaviour
{
    public Image imageToFade;
    public float fadeDuration = 2f;
    public bool minimapOn = false;
    public Animator minimapAnimator;

    private void Start()
    {
        StartCoroutine(FadeOut());
    }

    private void Update()
    {
    }

    public IEnumerator FadeOut()
    {
        float timeElapsed = 0f;
        Color initialColor = imageToFade.color;

        while (timeElapsed < fadeDuration)
        {
            timeElapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(initialColor.a, 0f, timeElapsed / fadeDuration);
            imageToFade.color = new Color(initialColor.r, initialColor.g, initialColor.b, alpha);
            yield return null;
        }

        imageToFade.color = new Color(initialColor.r, initialColor.g, initialColor.b, 0f);

        minimapOn = true;

        if (minimapAnimator != null)
        {
            minimapAnimator.SetBool("On", true);
        }
    }

    public IEnumerator FadeIn(float delay)
    {
        float timeElapsed = 0f;
        Color initialColor = imageToFade.color;

        while (timeElapsed < fadeDuration + delay)
        {
            timeElapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(initialColor.a, 1f, timeElapsed / fadeDuration);
            imageToFade.color = new Color(initialColor.r, initialColor.g, initialColor.b, alpha);
            yield return null;
        }

        imageToFade.color = new Color(initialColor.r, initialColor.g, initialColor.b, 1f);

        minimapOn = false;

        if (minimapAnimator != null)
        {
            minimapAnimator.SetBool("On", false);
        }
    }
}