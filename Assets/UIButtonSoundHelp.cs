using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonSoundHelp : UIButtonInteractions
{

    private void Start()
    {
        if (GameManager.Instance != null && tickGameObject != null)
            tickGameObject.SetActive(GameManager.Instance.SoundHelpActive ? true : false);
    }
    public override void BeatUIToggle()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.SoundHelpActive = !GameManager.Instance.SoundHelpActive;

        tickGameObject.SetActive(GameManager.Instance.SoundHelpActive ? true : false);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        BeatUIToggle();
    }
}
