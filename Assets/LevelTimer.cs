using UnityEngine;
using TMPro;

public class LevelTimer : MonoBehaviour
{
    public TextMeshProUGUI uiText;
    private float sessionTime;
    private Animator animator;
    void Start()
    {
        uiText = GetComponent<TextMeshProUGUI>();
        animator = GetComponent<Animator>();
        sessionTime = 0f;
        UpdateSessionTime();
    }

    void Update()
    {
        if (GameManager.Instance.IsExitTransitionActive) return;

        sessionTime += Time.deltaTime;
        GameManager.Instance.TotalTime += Time.deltaTime;
        GameManager.Instance.CurrentLevelTime = sessionTime;
        UpdateSessionTime();
    }

    void UpdateSessionTime()
    {
        int hours = Mathf.FloorToInt(sessionTime / 3600);
        int minutes = Mathf.FloorToInt((sessionTime % 3600) / 60);
        int seconds = Mathf.FloorToInt(sessionTime % 60);

        uiText.text = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
    }
}
