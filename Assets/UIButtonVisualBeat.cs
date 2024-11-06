using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonVisualBeat : UIButtonInteractions
{
    private void Start()
    {
        if (GameManager.Instance != null && tickGameObject != null)
            tickGameObject.SetActive(GameManager.Instance.BeatUIHelpActive ? true : false);
    }
    public override void BeatUIToggle()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.BeatUIHelpActive = !GameManager.Instance.BeatUIHelpActive;

        tickGameObject.SetActive(GameManager.Instance.BeatUIHelpActive? true : false);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        BeatUIToggle();
    }
}
