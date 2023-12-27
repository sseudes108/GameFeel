using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour{
    public static Action OnPlayerHit;
    
    [SerializeField] private float _jumpForce = 7f;
    [SerializeField] private float _jumpInterval = 4f;
    [SerializeField] private float _changeDirectionInterval = 3f;

    private int _currentDirection;
    private Rigidbody2D _rigidBody;
    private Movement _movement;
    private ColorChanger _colorChanger;
    private int _damageAmount = 1;
    private float _knockbackThrust = 25f;

    private void Awake(){
        _rigidBody = GetComponent<Rigidbody2D>();
        _movement = GetComponent<Movement>();
        _colorChanger = GetComponent<ColorChanger>();
    }

    private void Start() {
        StartCoroutine(ChangeDirection());
        StartCoroutine(RandomJump());
    }

    private void Update(){
        Movement();
    }

    private void OnCollisionEnter2D(Collision2D other) {
        PlayerController player = other.gameObject.GetComponent<PlayerController>();
        Movement playerMovement = other.gameObject.GetComponent<Movement>();

        if(!player) return;

        if(playerMovement.CanMove){
            IHitable iHitable = other.gameObject.GetComponent<IHitable>();
            iHitable?.TakeHit();

            IDamageable iDamageble = other.gameObject.GetComponent<IDamageable>();
            iDamageble?.TakeDamage(transform.position, _damageAmount, _knockbackThrust);

            OnPlayerHit?.Invoke();
        }
    }

    public void Init(Color color){
        _colorChanger.SetDefaultColor(color);
        transform.SetParent(GeneralManager.Instance.EnemyManager);
    }

    private void Movement(){
        _movement.SetMoveDirection(_currentDirection);
    }

    private IEnumerator ChangeDirection(){
        while (true){
            _currentDirection = UnityEngine.Random.Range(0, 2) * 2 - 1; // 1 or -1
            yield return new WaitForSeconds(_changeDirectionInterval);
        }
    }

    private IEnumerator RandomJump(){
        while (true){
            yield return new WaitForSeconds(_jumpInterval);
            float randomDirection = UnityEngine.Random.Range(-1, 1);
            Vector2 jumpDirection = new Vector2(randomDirection, 1f).normalized;
            _rigidBody.AddForce(jumpDirection * _jumpForce, ForceMode2D.Impulse);
        }
    }

}