using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonExit : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Application.Quit();
    }
}
