using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject _bulletImpactVFX;
    private Rigidbody2D _rigidBody;
    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private int _damageAmount = 1;
    private float _knockbackThrust = 20f;
    private Vector2 _fireDirection;
    private Gun _gun;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    public void Init(Gun gun, Vector2 bulletSpawnPosition, Vector2 _mousePos){
        _gun = gun;
        transform.position = bulletSpawnPosition;
        _fireDirection = (_mousePos - bulletSpawnPosition).normalized;
    }

    private void FixedUpdate(){
        _rigidBody.velocity = _fireDirection * _moveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other) {

        IHitable iHitable = other.GetComponent<IHitable>();
        iHitable?.TakeHit();

        IDamageable iDamageble = other.GetComponent<IDamageable>();
        iDamageble?.TakeDamage(_damageAmount, _knockbackThrust);

        Instantiate(_bulletImpactVFX, transform.position, Quaternion.identity);
        
        _gun.ReleaseFromPool(this);
    }
}