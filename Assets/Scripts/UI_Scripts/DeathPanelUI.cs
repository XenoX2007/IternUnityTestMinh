using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DeathPanelUI : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button retryButton;
    [SerializeField] private Button skipButton;

    [Header("Optional")]
    [SerializeField] private TextMeshProUGUI subText;
    

    private string[] _deathMessages =
    {
        "Better luck next time!",
        "So close...",
        "Try again!",
        "You can do it!",
        "Don't give up!"
    };

    private void Awake()
    {
        retryButton?.onClick.AddListener(OnRetry);
        skipButton?.onClick.AddListener(OnSkip);
    }

    
    private void OnEnable()
    {
        if (subText)
            subText.text = _deathMessages[Random.Range(0, _deathMessages.Length)];

        
    }
    private void OnRetry()
{
    Debug.Log("Retry clicked"); 
    LevelManager.Instance?.ReloadCurrentLevel();
}

    private void OnSkip()
{
    int current = LevelManager.Instance?.GetCurrentLevel() ?? 1;
    int next    = Mathf.Min(current + 1, LevelManager.TOTAL_LEVELS);

    
    LevelManager.Instance?.SaveProgress(next);
    LevelManager.Instance?.LoadLevel(next);
}
}