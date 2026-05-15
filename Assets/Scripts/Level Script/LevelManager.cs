using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    public const int TOTAL_LEVELS    = 20; // total shown in level select grid
    public const int PLAYABLE_LEVELS = 3;  // actual scenes that exist

    private const string SAVE_KEY       = "SavedLevel";
    private const string FIRST_TIME_KEY = "HasPlayedBefore";

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (!PlayerPrefs.HasKey(FIRST_TIME_KEY))
        {
            PlayerPrefs.SetInt(SAVE_KEY, 1);
            PlayerPrefs.SetInt(FIRST_TIME_KEY, 1);
            PlayerPrefs.Save();
        }
    }

    public int GetCurrentLevel() => PlayerPrefs.GetInt(SAVE_KEY, 1);

    public void SaveProgress(int level)
    {
        PlayerPrefs.SetInt(SAVE_KEY, Mathf.Clamp(level, 1, TOTAL_LEVELS));
        PlayerPrefs.Save();
    }

    public void LoadLevel(int level)
    {
        level = Mathf.Clamp(level, 1, TOTAL_LEVELS);
        SaveProgress(level);
        GameManager.Instance?.SetState(GameState.Playing);

        // Only load scene if it exists, otherwise reload last playable level
        if (level <= PLAYABLE_LEVELS)
            SceneManager.LoadScene(level); // scene index: Level1=1, Level2=2, Level3=3
        else
            SceneManager.LoadScene(PLAYABLE_LEVELS); // fallback to last real scene
    }

    public void LoadNextLevel()
    {
        int next = GetCurrentLevel() + 1;
        if (next > TOTAL_LEVELS)
        {
            SceneManager.LoadScene(0); // back to Lobby
            GameManager.Instance?.SetState(GameState.Lobby);
        }
        else
        {
            SaveProgress(next);
            LoadLevel(next);
        }
    }

    public void ReloadCurrentLevel()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        GameManager.Instance?.SetState(GameState.Playing);
        SceneManager.LoadScene(currentScene);
    }

    public void LoadLobby()
    {
        SceneManager.LoadScene(0); // Lobby is always scene index 0
        GameManager.Instance?.SetState(GameState.Lobby);
    }
}