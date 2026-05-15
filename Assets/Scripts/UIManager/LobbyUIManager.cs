using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LobbyUIManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject levelSelectPanel;

    [Header("Main Menu")]
    [SerializeField] private Button playButton;

    [Header("Level Select")]
    [SerializeField] private Transform  levelGrid;
    [SerializeField] private GameObject levelButtonPrefab;
    [SerializeField] private Button     backButton;

    [Header("Level Button Sprites")]
    [SerializeField] private Sprite unlockedSprite;
    [SerializeField] private Sprite currentSprite;
    [SerializeField] private Sprite lockedSprite;

    // ✅ ADD — listen to GameManager state
    private void OnEnable()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnStateChanged += OnStateChanged;
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnStateChanged -= OnStateChanged;
    }

    // ✅ ADD — handle state changes
    private void OnStateChanged(GameState state)
    {
        if (state == GameState.Lobby)
        {
            ShowMainMenu();
            BuildLevelGrid(); // refresh grid with latest saved progress
        }
    }

    private void Start()
    {
        // ✅ ADD — set lobby state when scene loads
        GameManager.Instance?.SetState(GameState.Lobby);

        ShowMainMenu();
        playButton?.onClick.AddListener(ShowLevelSelect);
        backButton?.onClick.AddListener(ShowMainMenu);
        BuildLevelGrid();
    }

    private void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);
        levelSelectPanel.SetActive(false);
    }

    private void ShowLevelSelect()
    {
        mainMenuPanel.SetActive(false);
        levelSelectPanel.SetActive(true);
        BuildLevelGrid(); // ✅ refresh every time level select opens
    }

    private void BuildLevelGrid()
    {
        if (levelGrid == null)        { Debug.LogError("LevelGrid not assigned!"); return; }
        if (levelButtonPrefab == null) { Debug.LogError("LevelButtonPrefab not assigned!"); return; }

        int savedLevel  = LevelManager.Instance?.GetCurrentLevel() ?? 1;
        int totalLevels = LevelManager.TOTAL_LEVELS;

        foreach (Transform child in levelGrid)
            Destroy(child.gameObject);

        for (int i = 1; i <= totalLevels; i++)
        {
            int level = i;
            GameObject btn = Instantiate(levelButtonPrefab, levelGrid);

            Button          button = btn.GetComponent<Button>();
            Image           bg     = btn.GetComponent<Image>();
            TextMeshProUGUI label  = btn.GetComponentInChildren<TextMeshProUGUI>();

            label.text = level.ToString();

            bool isCurrent  = level == savedLevel;
            bool isUnlocked = level <= savedLevel;

            if (isCurrent)       bg.sprite = currentSprite;
            else if (isUnlocked) bg.sprite = unlockedSprite;
            else                 bg.sprite = lockedSprite;

            button.interactable = isUnlocked;

            if (isUnlocked)
                button.onClick.AddListener(() => LevelManager.Instance?.LoadLevel(level));
        }
    }
}