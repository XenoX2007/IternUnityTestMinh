using UnityEngine;

public class RollingLog : MonoBehaviour
{
    [Header("Roll Settings")]
    [SerializeField] private float rollSpeed    = 5f;
    [SerializeField] private float spawnOffsetX = 8f;  // how far right it spawns

    [Header("Delay")]
    [SerializeField] private float startDelay  = 0f;   // stagger multiple logs

    private Vector3 _startPos;
    private bool    _started;
    private float   _timer;

    private void Awake()
    {
        _startPos = transform.position;
    }

    private void Start()
    {
        // Place at spawn point immediately
        transform.position = new Vector3(
            _startPos.x + spawnOffsetX,
            _startPos.y,
            _startPos.z);
    }

    private void Update()
    {
        // Handle start delay
        if (!_started)
        {
            _timer += Time.deltaTime;
            if (_timer >= startDelay)
                _started = true;
            return;
        }

        // Roll left (negative X)
        transform.position += Vector3.left * rollSpeed * Time.deltaTime;

        // Spin on Z axis to match rolling left
        float spinDeg = rollSpeed * (180f / Mathf.PI) * Time.deltaTime;
        transform.Rotate(Vector3.forward, spinDeg, Space.World);

        // When log passes far left, reset to right side
        float distanceTravelled = _startPos.x + spawnOffsetX - transform.position.x;
        if (distanceTravelled >= spawnOffsetX * 2f)
        {
            transform.position = new Vector3(
                _startPos.x + spawnOffsetX,
                _startPos.y,
                _startPos.z);
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.TryGetComponent<PlayerMovement>(out var player))
            player.TriggerDeath();
    }
}