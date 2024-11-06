using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class UIButtonInteractions : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] protected GameObject tickGameObject;

    public abstract void BeatUIToggle();

    public abstract void OnPointerClick(PointerEventData eventData);
}
