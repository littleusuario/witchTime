using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonInteractions : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] GameObject tickGameObject;

    public void BeatUIToggle()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.BeatUIHelpActive = !GameManager.Instance.BeatUIHelpActive;

        tickGameObject.SetActive(GameManager.Instance.BeatUIHelpActive ? true : false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        BeatUIToggle();
    }
}
