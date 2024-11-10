using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelName : MonoBehaviour
{
    private TextMeshProUGUI textMeshPro;

    private void Start()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
        UpdateText();
    }

    private void UpdateText()
    {
        if (textMeshPro != null)
        {
            int pisoNumero = GameManager.Instance.iterations + 1;
            textMeshPro.text = "Piso " + pisoNumero;
        }
    }
}
