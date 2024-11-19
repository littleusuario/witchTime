using TMPro;
using UnityEngine;

public class DifficultyUIText : MonoBehaviour
{
    public Color EasyDifficultyColor = new Color32(r: 168, g: 209, b: 66, a: 56);
    public Color MediumDifficultyColor = new Color32(r: 209, g: 177, b: 66, a: 56);
    public Color HardDifficultyColor = new Color32(r: 209, g: 80, b: 66, a: 56);

    private TextMeshProUGUI uiText = null;
    void Start()
    {
        uiText = GetComponent<TextMeshProUGUI>();
        if (uiText != null)
        {
            uiText.text = GameManager.Instance.DifficultyLevel.ToString();
        }
        int difficulty = GameManager.Instance.DifficultyLevel;
        
        if (difficulty < 3) 
        {
            uiText.color = EasyDifficultyColor;
        }
        else if ( difficulty < 5)
        {
            uiText.color = MediumDifficultyColor;    
        }
        else if (difficulty < 999) 
        {
            uiText.color = HardDifficultyColor;
        }
    }
}
