using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIResponseToBeat : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float pulseSize;
    public bool AndresSpecial;
    public float distance = 0;

    List<TextMeshProUGUI> UITexts = new List<TextMeshProUGUI>();
    List<PulseToTheBeat> PulseToTheBeatList = new List<PulseToTheBeat>();
    private void Start()
    {
        foreach (Transform child in transform)
        {
            UITexts.Add(child.GetComponent<TextMeshProUGUI>());
        }

        foreach (Transform child in transform)
        {
            if (child.gameObject.TryGetComponent(out PulseToTheBeat pulse))
            {
                PulseToTheBeatList.Add(pulse);
            }
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (AndresSpecial) 
        {
            foreach(TextMeshProUGUI text in UITexts) 
            {
                StopCoroutine("OpacityChange");
                StartCoroutine(OpacityChange(text, new Color(1, 1, 1, 0.5f)));
            }
        }
            foreach (PulseToTheBeat pulse in PulseToTheBeatList)
            {
                pulse.PulseSize = pulseSize;
            }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (AndresSpecial) 
        {
            foreach (TextMeshProUGUI text in UITexts)
            {
                StopCoroutine("OpacityChange");
                StartCoroutine(OpacityChange(text, new Color(1, 1, 1, 0)));
            }
        }

        foreach (PulseToTheBeat pulse in PulseToTheBeatList)
        {
            pulse.PulseSize = 1;
        }
    }

    IEnumerator OpacityChange(TextMeshProUGUI text, Color desiredColor) 
    {
        Color initialColor = text.color;
        float elapsedTime = 0;

        while (elapsedTime < 0.5) 
        {
            elapsedTime += Time.deltaTime;
            text.color = Color.Lerp(initialColor, desiredColor, elapsedTime / 0.68f);
            yield return null;
        }
        yield return null;
        text.color = desiredColor;
    }
}
