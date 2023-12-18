using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private float _jumpForce = 7f;
    [SerializeField] private float _jumpInterval = 4f;
    [SerializeField] private float _changeDirectionInterval = 3f;

    private int _currentDirection;
    private float _knockbackThrust;

    private Rigidbody2D _rigidBody;
    private Movement _movement;
    private ColorChanger _colorChanger;
    private Knockback _knockback;
    private Flash _flash;
    private Health _health;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _movement = GetComponent<Movement>();
        _colorChanger = GetComponent<ColorChanger>();
        _flash = GetComponent<Flash>();
        _health = GetComponent<Health>();
        _knockback = GetComponent<Knockback>();
    }

    private void Start() {
        StartCoroutine(ChangeDirection());
        StartCoroutine(RandomJump());
    }

    public void Init(Color color){
        _colorChanger.SetDefaultColor(color);
        transform.SetParent(GeneralManager.Instance.EnemyManager);
    }

    private void Update()
    {
        Movement();
    }

    private void Movement(){
        _movement.SetMoveDirection(_currentDirection);
    }

    private IEnumerator ChangeDirection()
    {
        while (true){
            _currentDirection = UnityEngine.Random.Range(0, 2) * 2 - 1; // 1 or -1
            yield return new WaitForSeconds(_changeDirectionInterval);
        }
    }

    private IEnumerator RandomJump() 
    {
        while (true)
        {
            yield return new WaitForSeconds(_jumpInterval);
            float randomDirection = Random.Range(-1, 1);
            Vector2 jumpDirection = new Vector2(randomDirection, 1f).normalized;
            _rigidBody.AddForce(jumpDirection * _jumpForce, ForceMode2D.Impulse);
        }
    }

    //Interfaces
    public void TakeDamage(int damageAmount, float knockbackThrust){
        _health.TakeDamage(damageAmount);
        _knockback.GetKnockedBack(PlayerController.Instance.transform.position, knockbackThrust);
    }
    public void TakeHit(){
       _flash.StartFlash();
    }
}