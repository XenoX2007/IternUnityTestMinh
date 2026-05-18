using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpHeight = 3.8f;
    [SerializeField] private float gravity = -25f;
    
    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundRadius = 0.25f;
    [SerializeField] private LayerMask groundMask;

    private CharacterController _cc;
    private InputSystem_Actions _controls;
    private Vector3 _velocity;
    private bool _isGrounded;
    private PlayerAnimator _playerAnimator;
    // Knockback
    private Vector3 _knockbackVelocity;

    public Vector2 CurrentInput
    {
        get
        {
            Vector2 kb = _controls.Player.Move.ReadValue<Vector2>();
            return kb.sqrMagnitude > 0.01f ? kb : MobileMove;
        }
    }


    // Mobile input (set by MobileControlsManager or OnScreenStick)
    public Vector2 MobileMove { get; set; }
    public bool MobileJumpPressed { get; set; }

    private void Awake()
    {
        _cc = GetComponent<CharacterController>();
        _controls = new InputSystem_Actions();
        _playerAnimator = GetComponentInChildren<PlayerAnimator>();
    }

    private void OnEnable()  => _controls.Enable();
    private void OnDisable() => _controls.Disable();

    private void Update()
{
    
    // Allow movement if Playing OR if GameManager doesn't exist (direct scene testing)
    bool canMove = GameManager.Instance == null || 
                   GameManager.Instance.CurrentState == GameState.Playing;
    if (!canMove) return;

    CheckGround();
    HandleMove();
    HandleJump();
    ApplyGravity();
    
}
private Vector3 _treePushVelocity;

// REPLACE OnControllerColliderHit
private void OnControllerColliderHit(ControllerColliderHit hit)
{
    if (hit.gameObject.TryGetComponent<TreeObstacle>(out var tree))
    {
        // Carry player in exact direction tree is moving
        _treePushVelocity = tree.MoveDirection;
    }
}
    private void CheckGround()
    {
        _isGrounded = Physics.CheckSphere(groundCheck.position, groundRadius, groundMask);
        if (_isGrounded && _velocity.y < 0f)
            _velocity.y = -2f;
    }

    // REPLACE HandleMove()
private void HandleMove()
{
    Vector2 kb    = _controls.Player.Move.ReadValue<Vector2>();
    Vector2 input = kb.sqrMagnitude > 0.01f ? kb : MobileMove;

    bool isPushed = _treePushVelocity.magnitude > 0.1f;

    // Block player input while tree is pushing
    Vector3 move = isPushed
        ? _treePushVelocity                                      // only tree push
        : new Vector3(input.x, 0f, input.y) * moveSpeed;        // normal movement

    move.y = _velocity.y;
    _cc.Move(move * Time.deltaTime);
    this.transform.LookAt (new Vector3(-move.z,
                                        this.transform.position.y,
                                        move.x));
    // Decay push velocity so it doesn't linger after contact ends
    _treePushVelocity = Vector3.Lerp(_treePushVelocity, Vector3.zero, 15f * Time.deltaTime);
}
    private void HandleJump()
    {
        bool jumpKb     = _controls.Player.Jump.WasPressedThisFrame();
        bool jumpMobile = MobileJumpPressed;
        MobileJumpPressed = false;

        if ((jumpKb || jumpMobile) && _isGrounded)
            _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }
//
    private void ApplyGravity()
    {
        _velocity.y += gravity * Time.deltaTime;
    }

    

    
public void ApplyKnockback(Vector3 force)
{
    _knockbackVelocity = force;          // instant burst, no timer delay
    _velocity.y        = Mathf.Sqrt(Mathf.Abs(force.y) * -2f * gravity); // pop upward
}

  public void TriggerDeath()
{
    if (GameManager.Instance?.CurrentState != GameState.Playing) return;
    GameManager.Instance.SetState(GameState.GameOver);

    _cc.enabled      = false;
    this.enabled     = false;
}
}