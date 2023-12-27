using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour{
    public static PlayerController Instance;
    public static Action OnJump, OnJetPack;
    public bool IsGrounded => CheckGround();
    public FrameInput FrameInput  => _frameInput;

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
    [SerializeField] private float  _maxFallSpeedVelocity = -50f;
    private float _timeInAir;

    //Coyote
    [SerializeField] private float _coyoteTime;
    private float _coyoteCounter;

    //Jetpack
    [SerializeField] private float _jetpackTimer = 0.6f;
    [SerializeField] private TrailRenderer _jetpackTrailRenderer;
    [SerializeField] private float _jetPackStrength = 12f;
    private Coroutine _jetpackCoroutine;

#region Unity Methods
    private void Awake() {
        if (Instance == null) {Instance = this;}

        _rigidBody = GetComponent<Rigidbody2D>();
        _inputs = GetComponent<InputManager>();
        _movement = GetComponent<Movement>();
    }
    private void OnEnable() {
        OnJump += ApplyJumpForce;
        OnJetPack += StartJetpack;
    }
    private void OnDisable() {
        OnJump -= ApplyJumpForce;
        OnJetPack -= StartJetpack;
    }

    private void OnDestroy() {
        FadeScreen fade = FindFirstObjectByType<FadeScreen>();
        if(fade != null){fade.FadeInAndOut();}
    }

    private void Update(){
        GatherInput();
        HandleJump();
        Movement();
        GravityDelay();
        CoyoteTimer();
        HandleSpriteFlip();
        Jetpack();
    }

    private void FixedUpdate() {
        ExtraGravity();
    }
#endregion

    private void GatherInput(){
        _frameInput = _inputs.FrameInput;
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

#region Movement
    private void Movement(){
        _movement.SetMoveDirection(_frameInput.Move.x);
    }

    private bool CheckGround(){
        Collider2D isGrounded = Physics2D.OverlapBox(_groundCheckPoint.position, _groundCheckSize, 0f, _groundLayer);
        return isGrounded;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_groundCheckPoint.position, _groundCheckSize);
    }
#endregion

#region Jump

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

            if(_rigidBody.velocity.y < _maxFallSpeedVelocity){
                _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, _maxFallSpeedVelocity);
            }
        }
    }

#endregion

#region Jetpack
    private void Jetpack(){
        if(!_frameInput.JetPack || _jetpackCoroutine != null) return;

        OnJetPack?.Invoke();
    }

    private void StartJetpack(){
        _jetpackTrailRenderer.emitting = true;
        _jetpackCoroutine = StartCoroutine(JetpackRoutine());
    }

    private IEnumerator JetpackRoutine(){
        float jetTime  = 0f;

        while (jetTime < _jetpackTimer){
            jetTime += Time.deltaTime;
            _rigidBody.velocity = Vector2.up * _jetPackStrength;
            yield return null;
        }
        _jetpackTrailRenderer.emitting = false;
        _jetpackCoroutine = null;
    }
#endregion

}
