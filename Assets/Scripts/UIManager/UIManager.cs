using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject gameOverPanel;

    private bool _subscribed;

    private void Start()
    {
        gameOverPanel.SetActive(false);

        if (!_subscribed && GameManager.Instance != null)
        {
            GameManager.Instance.OnStateChanged += OnStateChanged;
            _subscribed = true;
        }

        OnStateChanged(GameManager.Instance?.CurrentState ?? GameState.Playing);
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnStateChanged -= OnStateChanged;
    }

    private void OnStateChanged(GameState state)
    {
        gameOverPanel.SetActive(state == GameState.GameOver);
    }
}