using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float endTime = 0.2f;
    Vector3 endScaleCanvas = Vector3.zero;
    Vector3 startScaleCanvas = Vector3.zero;
    
    void Start()
    {
        endScaleCanvas = transform.localScale * 1.1f;
        startScaleCanvas = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StartCoroutine(ScaleButton(endScaleCanvas));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StartCoroutine(ScaleButton(startScaleCanvas));
    }

    IEnumerator ScaleButton(Vector3 endScale)
    {
        float elapsedTime = 0;
        Vector3 startScale = transform.localScale;

        while (elapsedTime < endTime)
        {
            elapsedTime += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startScale, endScale, elapsedTime / endTime);
            yield return null;
        }
        transform.localScale = endScale;
    }
}
