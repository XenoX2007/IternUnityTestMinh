using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private GameObject capsuleModel;

    private Animator            _anim;
    private PlayerMovement      _movement;
    private CharacterController _cc;

    private static readonly int IsMoving  = Animator.StringToHash("isMoving");
    private static readonly int IsJumping = Animator.StringToHash("isJumping");
    private static readonly int IsDead    = Animator.StringToHash("isDead");

    private bool  _isDead;
    private float _groundedTimer;
    private const float GroundedBuffer = 0.15f;

    private void Awake()
    {
        _anim     = GetComponentInChildren<Animator>(true);
        _movement = GetComponentInParent<PlayerMovement>();
        _cc       = GetComponentInParent<CharacterController>();

        if (!_anim)     Debug.LogError("PlayerAnimator: Animator not found!");
        if (!_movement) Debug.LogError("PlayerAnimator: PlayerMovement not found!");
        if (!_cc)       Debug.LogError("PlayerAnimator: CharacterController not found!");
    }

    private void Update()
    {
        // Check death every frame
        if (!_isDead && GameManager.Instance?.CurrentState == GameState.GameOver)
        {
            TriggerDie();
            return;
        }

        if (_isDead) return;

        Vector2 moveInput = _movement.CurrentInput;

        // Grounded buffer prevents flickering
        if (_cc.isGrounded)
            _groundedTimer += Time.deltaTime;
        else
            _groundedTimer  = 0f;

        bool isGrounded = _groundedTimer >= GroundedBuffer;
        bool moving     = moveInput.sqrMagnitude > 0.01f && isGrounded;
        bool jumping    = !_cc.isGrounded;

        _anim.SetBool(IsMoving,  moving);
        _anim.SetBool(IsJumping, jumping);
    }

    private void TriggerDie()
    {
        _isDead = true;

        _anim.SetBool(IsMoving,  false);
        _anim.SetBool(IsJumping, false);
        _anim.SetBool(IsDead,    true);

        float clipLength = GetClipLength("Die");
        Invoke(nameof(HideModel), clipLength > 0 ? clipLength : 1f);
    }

    private void HideModel()
    {
        if (capsuleModel)
            capsuleModel.SetActive(false);
    }

    private float GetClipLength(string clipName)
    {
        if (_anim.runtimeAnimatorController == null) return 1f;
        foreach (var clip in _anim.runtimeAnimatorController.animationClips)
            if (clip.name.Contains(clipName))
                return clip.length;
        return 1f;
    }
}