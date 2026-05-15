using UnityEngine;

public class SpinningLog : MonoBehaviour
{
    [Header("Spin Settings")]
    [SerializeField] private float spinSpeed      = 4f;
    [SerializeField] private float spinDistance   = 6f; // how far it travels
    [SerializeField] private bool  pingPong       = true; // back and forth or loop

    [Header("Spin Axis")]
    [SerializeField] private Vector3 rollDirection = Vector3.forward; // direction it moves
    [SerializeField] private Vector3 spinAxis      = Vector3.right;   // axis it spins on

    private Vector3 _startPos;
    private float   _travelled;
    private int     _dir = 1;

    private void Awake()
    {
        _startPos = transform.position;
    }

    private void Update()
    {
        // Move
        Vector3 move = rollDirection.normalized * spinSpeed * _dir * Time.deltaTime;
        transform.position += move;

        // Spin to match movement
        float spinDeg = spinSpeed * _dir * (180f / Mathf.PI) * Time.deltaTime;
        transform.Rotate(spinAxis, spinDeg, Space.World);

        // Track distance
        _travelled += spinSpeed * Time.deltaTime;

        if (_travelled >= spinDistance)
        {
            _travelled = 0f;

            if (pingPong)
            {
                _dir *= -1; // reverse
            }
            else
            {
                // Loop back to start silently
                transform.position = _startPos;
            }
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.TryGetComponent<PlayerMovement>(out var player))
            player.TriggerDeath();
    }
}