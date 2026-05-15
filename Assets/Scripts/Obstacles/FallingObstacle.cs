using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class FallingObstacle : MonoBehaviour
{
    [SerializeField] private float waitBeforeFall  = 3f;
    [SerializeField] private float resetAfterFall  = 2.5f;
    [SerializeField] private float warningDuration = 0.6f;

    private Rigidbody   _rb;
    private Renderer    _rend;
    private Vector3     _origin;

    private void Awake()
    {
        _rb     = GetComponent<Rigidbody>();
        _rend   = GetComponentInChildren<Renderer>();
        _origin = transform.position;
        _rb.isKinematic = true;
    }

    private void Start() => StartCoroutine(Cycle());

    private IEnumerator Cycle()
    {
        while (true)
        {
            yield return new WaitForSeconds(waitBeforeFall);
            yield return StartCoroutine(WarnFlash());

            _rb.isKinematic = false;           // Drop!

            yield return new WaitForSeconds(resetAfterFall);

            _rb.isKinematic = true;
            _rb.linearVelocity      = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
            transform.position  = _origin;
            transform.rotation  = Quaternion.identity;
        }
    }

    private IEnumerator WarnFlash()
    {
        float t = 0f;
        while (t < warningDuration)
        {
            _rend.enabled = !_rend.enabled;
            yield return new WaitForSeconds(0.1f);
            t += 0.1f;
        }
        _rend.enabled = true;
    }

    private void OnCollisionEnter(Collision col)
    {
        if (!_rb.isKinematic && col.gameObject.TryGetComponent<PlayerMovement>(out var p))
            p.TriggerDeath();
    }
}