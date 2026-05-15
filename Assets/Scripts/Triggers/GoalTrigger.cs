using UnityEngine;
using System.Collections;

public class GoalTrigger : MonoBehaviour
{
    [SerializeField] private float delayBeforeLoad = 2.5f;

    private bool _triggered;

    private void OnTriggerEnter(Collider other)
    {
        if (_triggered) return;
        if (!other.TryGetComponent<PlayerMovement>(out _)) return;

        _triggered = true;
        GameManager.Instance?.SetState(GameState.LevelComplete);
        StartCoroutine(LoadNext());
    }

    private IEnumerator LoadNext()
    {
        yield return new WaitForSeconds(delayBeforeLoad);
        LevelManager.Instance?.LoadNextLevel();
    }
}