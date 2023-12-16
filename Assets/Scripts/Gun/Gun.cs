using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.Pool;

public class Gun : MonoBehaviour
{
    private Action OnShot;

    private Animator _animator;
    private static readonly int FIRE = Animator.StringToHash("Fire");
    private CinemachineImpulseSource _impulseSource;

    public Transform BulletSpawnPoint => _bulletSpawnPoint;
    [SerializeField] private Transform _bulletSpawnPoint;
    [SerializeField] private Bullet _bulletPrefab;
    private Vector2 _mousePos;

    [SerializeField] private float _fireCD = 0.5f;
    private float _lastFireTime;

    private ObjectPool<Bullet> _bulletPool;

    private void Awake() {
        _animator = GetComponent<Animator>();
        _impulseSource = GetComponent<CinemachineImpulseSource>();
        CreateBulletPool();
    }
    
    private void Update(){
        Shoot();
        RotateGun();
        AutomaticFire();
    }

    private void OnEnable() {
        OnShot += ShootProjectile;
        OnShot += FireAnimation;
    }

    private void OnDisable() {
        OnShot -= ShootProjectile;
        OnShot -= FireAnimation;
    }

    //Shot and Projectile
    private void Shoot(){
        if (Input.GetMouseButton(0) && _lastFireTime >= _fireCD) {
            OnShot?.Invoke();
        }
    }

    private void ShootProjectile(){
        ScreenShake();
        Bullet newBullet = _bulletPool.Get();
        newBullet.Init(this, _bulletSpawnPoint.position, _mousePos);
        _lastFireTime = 0;
    }

    private void RotateGun(){
        _mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = PlayerController.Instance.transform.InverseTransformPoint(_mousePos);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.localRotation = Quaternion.Euler(0, 0, angle);
    }

    private void AutomaticFire(){
        _lastFireTime += Time.deltaTime;
    }

    private void FireAnimation(){
        _animator.Play(FIRE, 0, 0);
    }

    private void ScreenShake(){
        _impulseSource.GenerateImpulse();
    }

    //Pool
    private void CreateBulletPool(){
        _bulletPool = new ObjectPool<Bullet>(()=>{
            return Instantiate(_bulletPrefab);
        }, bullet =>{
            bullet.gameObject.SetActive(true);
        }, bullet =>{
            bullet.gameObject.SetActive(false);
        }, bullet =>{
            Destroy(bullet);
        },false, 20, 40){
        };
    }

    public void ReleaseFromPool(Bullet bullet){
        _bulletPool.Release(bullet);
    }
}
