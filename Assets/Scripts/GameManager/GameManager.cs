using UnityEngine;

public enum GameState { Lobby, Playing, GameOver, LevelComplete }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameState CurrentState { get; private set; } = GameState.Lobby;

    public event System.Action<GameState> OnStateChanged;

   private void Awake()
{
    if (Instance != null) { Destroy(gameObject); return; }
    Instance = this;
    DontDestroyOnLoad(gameObject);

    
    string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
    if (sceneName.StartsWith("Level"))
        CurrentState = GameState.Playing;
}

    public void SetState(GameState state)
    {
        CurrentState = state;
        OnStateChanged?.Invoke(state);
    }
}