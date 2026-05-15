using UnityEngine;

public class TreeTriggerZone : MonoBehaviour
{
    private TreeObstacle _tree;

    private void Awake()
    {
        _tree = GetComponentInParent<TreeObstacle>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerMovement>(out _))
            _tree.OnPlayerEnter(other.transform);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<PlayerMovement>(out _))
            _tree.OnPlayerExit();
    }//
}