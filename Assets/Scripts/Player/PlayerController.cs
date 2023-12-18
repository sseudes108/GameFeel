using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool IsGrounded => CheckGround();
    public FrameInput FrameInput  => _frameInput;
    
    public static PlayerController Instance;
    public static Action OnJump;
    
    //Components
    private Rigidbody2D _rigidBody;
    private Movement _movement;

    //Input
    private InputManager _inputs;
    private FrameInput _frameInput;

    //Move Vars
    [SerializeField] private float _jumpStrength = 7f;
    private bool _canDoubleJump;

    //Ground
    [SerializeField] Transform _groundCheckPoint;
    [SerializeField] LayerMask _groundLayer;
    [SerializeField] Vector2 _groundCheckSize;

    //Gravity
    [SerializeField] private float  _extraGravity = 700f;
    [SerializeField] private float _gravityDelay = 0.2f;
    private float _timeInAir;

    //Coyote
    [SerializeField] private float _coyoteTime;
    private float _coyoteCounter;

    private void Awake() {
        if (Instance == null) { Instance = this; }

        _rigidBody = GetComponent<Rigidbody2D>();
        _inputs = GetComponent<InputManager>();
        _movement = GetComponent<Movement>();
    }

    private void Update(){
        GatherInput();
        HandleJump();
        Movement();
        GravityDelay();
        CoyoteTimer();
        HandleSpriteFlip();
    }

    private void FixedUpdate() {
        ExtraGravity();
    }

    private void OnEnable() {
        OnJump += ApplyJumpForce;
    }

    private void OnDisable() {
        OnJump -= ApplyJumpForce;
    }

    private void GatherInput(){
        _frameInput = _inputs.FrameInput;
    }
    
    //Move
    private void Movement(){
        _movement.SetMoveDirection(_frameInput.Move.x);
    }

    //Ground
    private bool CheckGround(){
        Collider2D isGrounded = Physics2D.OverlapBox(_groundCheckPoint.position, _groundCheckSize, 0f, _groundLayer);
        return isGrounded;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_groundCheckPoint.position, _groundCheckSize);
    }

    //Jump
    private void HandleJump(){
        if(!_frameInput.Jump){return;};

        if(_coyoteCounter <= _coyoteTime){
            OnJump?.Invoke();
        }else if(_canDoubleJump){
            _timeInAir = 0;
            _canDoubleJump = false;
            OnJump?.Invoke();
        }
    }
    
    private void ApplyJumpForce(){
        _rigidBody.AddForce(Vector2.up * _jumpStrength, ForceMode2D.Impulse);
    }

    private void CoyoteTimer(){
        if(!CheckGround()){
            _coyoteCounter += Time.deltaTime;
        }else{
            _coyoteCounter = 0;
        }
    }

    //Extra Gravity
    private void GravityDelay(){
        if(!CheckGround()){
            _timeInAir += Time.deltaTime;
        }else{
            _canDoubleJump = true;
            _timeInAir = 0;
        }
    }

    private void ExtraGravity(){
        if(_timeInAir > _gravityDelay){
            _rigidBody.AddForce(new Vector2(0, -_extraGravity * Time.deltaTime));
        }
    }
    
    private void HandleSpriteFlip(){
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (mousePosition.x < transform.position.x){
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else{
            transform.localScale = new Vector3(1, 1, 1);
        }
    } 
}
