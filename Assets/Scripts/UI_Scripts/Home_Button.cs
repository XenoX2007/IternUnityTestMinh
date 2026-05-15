using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Home_Button : MonoBehaviour
{
    [SerializeField] GameObject homeButton;
    public void LoadLobby()
    {

        SceneManager.LoadScene("Lobby");
        GameManager.Instance?.SetState(GameState.Lobby);
    }
}
