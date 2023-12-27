using System;
using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.Pool;

public class Gun : MonoBehaviour
{
    public Transform BulletSpawnPoint => _bulletSpawnPoint;

    public static Action OnShot;

    private Animator _animator;
    private static readonly int FIRE = Animator.StringToHash("Fire");
    private CinemachineImpulseSource _impulseSource;
    private ObjectPool<Bullet> _bulletPool;
    
    [Header("Bullet")]
    [SerializeField] private Transform _bulletPoolManagerTransform;
    [SerializeField] private Transform _bulletSpawnPoint;
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private float _fireCD = 0.5f;
    private float _lastFireTime;
    private Vector2 _mousePos;

    [Header("Muzzle Flash")]
    [SerializeField] private GameObject _muzzleFlash;
    [SerializeField] private float _muzzleFlashTime;
    private Coroutine _muzzeFlashRoutine;

    [Header("Grenade")]
    [SerializeField] private Grenade _grenadePrefab;
    [SerializeField] private float _grenadeCD;
    private float _grenadeCDTimer;
    
#region Unity Methods
    private void Awake() {
        _animator = GetComponent<Animator>();
        _impulseSource = GetComponent<CinemachineImpulseSource>();
        CreateBulletPool();
    }
    
    private void Update(){
        RotateGun();
        AutomaticFire();
        Shoot();
        GrenadeLauncher(); 
        GrenadeCoolDown();      
    }

    private void OnEnable() {
        OnShot += ShootProjectile;
        OnShot += FireAnimation;
        OnShot += MuzzleFlash;

        Grenade.OnGrenadeLaunch += ShootGrenade;
        Grenade.OnGrenadeLaunch += FireAnimation;
    }

    private void OnDisable() {
        OnShot -= ShootProjectile;
        OnShot -= FireAnimation;
        OnShot -= MuzzleFlash;

        Grenade.OnGrenadeLaunch -= ShootGrenade;
        Grenade.OnGrenadeLaunch -= FireAnimation;
    }
#endregion

    private void RotateGun(){
        _mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = PlayerController.Instance.transform.InverseTransformPoint(_mousePos);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.localRotation = Quaternion.Euler(0, 0, angle);
    }

#region Shot And Grenade
    //Shot and Projectile
    private void Shoot(){
        if (PlayerController.Instance.FrameInput.Shot && _lastFireTime >= _fireCD) {
            OnShot?.Invoke();
        }
    }

    private void ShootProjectile(){
        ScreenShake();
        Bullet newBullet = _bulletPool.Get();
        newBullet.Init(this, _bulletSpawnPoint.position, _mousePos);
        _lastFireTime = 0;
    }

    private void AutomaticFire(){
        _lastFireTime += Time.deltaTime;
    }

    private void GrenadeCoolDown(){
        if(_grenadeCDTimer > 0){
            _grenadeCDTimer -= Time.deltaTime;
        }else{
            _grenadeCDTimer = 0;
        }
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

    //Muzzle Flash
    private void MuzzleFlash(){
        if(_muzzeFlashRoutine != null){
            StopCoroutine(_muzzeFlashRoutine);
        }
        
        _muzzeFlashRoutine = StartCoroutine(MuzzleFlashRoutine());
    }

    private IEnumerator MuzzleFlashRoutine(){
        _muzzleFlash.SetActive(true);
        yield return new WaitForSeconds(_muzzleFlashTime);
        _muzzleFlash.SetActive(false);
    }

    //Grenade
    private void GrenadeLauncher(){
        if(!PlayerController.Instance.FrameInput.Granade || _grenadeCDTimer != 0) return;
        Grenade.OnGrenadeLaunch?.Invoke();
    }

    private void ShootGrenade(){
        Grenade newGranade = Instantiate(_grenadePrefab);
        newGranade.Init(_bulletSpawnPoint.position, _mousePos);
        _grenadeCDTimer = _grenadeCD;
    }
#endregion

}
