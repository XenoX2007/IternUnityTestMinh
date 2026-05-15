using UnityEngine;

public class DropDownObstacle : MonoBehaviour
{
    [Header("Drop Settings")]
    [SerializeField] private float dropSpeed     = 6f;
    [SerializeField] private float returnSpeed   = 3f;
    [SerializeField] private float dropDistance  = 8f; // how far it falls down

    [Header("Delay")]
    [SerializeField] private float dropDelay     = 0.5f; // seconds before it drops

    private Vector3 _startPos;
    private Vector3 _targetPos;
    private bool    _playerInside;
    private float   _delayTimer;
    private bool    _dropping;

    public Vector3 MoveDirection { get; private set; }

    private void Awake()
    {
        _startPos  = transform.position;
        _targetPos = _startPos + Vector3.down * dropDistance; // drop downward
    }

    private void Update()
    {
        Vector3 prev = transform.position;

        if (_playerInside)
        {
            // Wait for delay before dropping
            _delayTimer += Time.deltaTime;
            if (_delayTimer >= dropDelay)
                _dropping = true;
        }
        else
        {
            _dropping   = false;
            _delayTimer = 0f;
        }

        if (_dropping)
        {
            // Drop down
            transform.position = Vector3.MoveTowards(
                transform.position, _targetPos, dropSpeed * Time.deltaTime);
        }
        else
        {
            // Return to start
            transform.position = Vector3.MoveTowards(
                transform.position, _startPos, returnSpeed * Time.deltaTime);
        }

        MoveDirection = (transform.position - prev) / Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerMovement>(out _))
            OnPlayerEnter();
    }

    

    private void OnPlayerEnter()
    {
        _playerInside = true;
    }

    
}