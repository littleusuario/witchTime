using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonSceneLoader : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string nameOfLoadLevel;
    [SerializeField] private float endTime = 0.2f;
    Button button;
    Vector3 endScaleCanvas = Vector3.zero;
    Vector3 startScaleCanvas = Vector3.zero;
    private void Awake()
    {
        button = GetComponent<Button>();

        if (button != null) 
        {
            button.onClick.AddListener(LoadLevel);
        }
        endScaleCanvas = transform.localScale * 1.1f;
        startScaleCanvas = transform.localScale;
    }
    public void LoadLevel() 
    {
        SceneManager.LoadScene(nameOfLoadLevel);
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
