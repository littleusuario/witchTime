using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonVisualBeat : UIButtonInteractions
{
    public AudioSource on;
    public AudioSource off;
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
        if (GameManager.Instance.BeatUIHelpActive == true)
        {
            on.Play();
        }
        else if (GameManager.Instance.BeatUIHelpActive == false)
        {
            off.Play();
        }
    }

   
}
