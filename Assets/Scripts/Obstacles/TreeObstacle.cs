using UnityEngine;

public class TreeObstacle : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed     = 6f;
    [SerializeField] private float returnSpeed   = 3f;
    [SerializeField] private float maxMoveDistance = 8f; // far enough to push off edge
    private TreeObstacle _tree;
    private Vector3 _startPos;
    private Vector3 _targetPos;
    private bool    _playerInside;

    public Vector3 MoveDirection { get; private set; } // read by PlayerMovement

    private void Awake()
    {
        _startPos  = transform.position;
        _targetPos = _startPos;
    }

    private void Update()
    {
        Vector3 prev = transform.position;

        if (_playerInside)
        {
            transform.position = Vector3.MoveTowards(
                transform.position, _targetPos, moveSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.MoveTowards(
                transform.position, _startPos, returnSpeed * Time.deltaTime);
        }

        // Track actual move direction this frame so player gets pushed correctly
        MoveDirection = (transform.position - prev) / Time.deltaTime;
    }

    public void OnPlayerEnter(Transform player)
    {
        _playerInside = true;

        Vector3 dir = (player.position - transform.position);
        dir.y  = 0f;
        _targetPos = _startPos + dir.normalized * maxMoveDistance;
    }//

    public void OnPlayerExit()
    {
        _playerInside = false;
    }
}