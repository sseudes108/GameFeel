using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Cinemachine;

public class Grenade : MonoBehaviour {

    public static Action OnGrenadeLaunch, OnGrenadeBeep, OnGrenadeExplode;

    [SerializeField] private float _launchForce;
    [SerializeField] private int _grenadeDamage = 3;
    [SerializeField] private float _explosionRange;
    [SerializeField] private GameObject _explosionVFX;
    [SerializeField] private LayerMask _enemyLayerMask;

    private Rigidbody2D _rigidbody;
    private Vector2 _fireDirection;

    private Vector2 _mousePos;
    private Vector2 _spawnPosition;

    private Light2D _light2D;
    [SerializeField] float _flashLightTime = 0.5f;

    private CinemachineImpulseSource _impulseSource;

    private void Awake() {
        _rigidbody = GetComponent<Rigidbody2D>();
        _light2D = GetComponentInChildren<Light2D>();
        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void Start() {
        Launch();
        FlashLight();
    }

    public void Init(Vector2 spawnPosition, Vector2 mousePos){
        _spawnPosition = spawnPosition;
        _mousePos = mousePos;

        transform.position = _spawnPosition;
    }

    private void Launch(){
        _fireDirection = (_mousePos - _spawnPosition).normalized;
        _rigidbody.AddForce(_fireDirection * _launchForce, ForceMode2D.Impulse);
        OnGrenadeLaunch?.Invoke();
    }

    private void FlashLight(){
        StartCoroutine(FlashLightRoutine());
    }

    private IEnumerator FlashLightRoutine(){
        TurnOff();
        yield return new WaitForSeconds(_flashLightTime);

        TurnOn();
        yield return new WaitForSeconds(_flashLightTime);

        TurnOff();
        yield return new WaitForSeconds(_flashLightTime);

        TurnOn();
        yield return new WaitForSeconds(_flashLightTime);

        TurnOff();
        yield return new WaitForSeconds(_flashLightTime);

        TurnOn();
        yield return new WaitForSeconds(_flashLightTime);

        ScreenShake();
        Explode();
    }

    private void TurnOn(){
        _light2D.enabled = true;
        OnGrenadeBeep?.Invoke();
    }

    private void TurnOff(){
        _light2D.enabled = false;
    }

    private void Explode(){
        OnGrenadeExplode?.Invoke();

        Collider2D[] enemiesInExplosionRange = Physics2D.OverlapCircleAll(transform.position, _explosionRange, _enemyLayerMask);
        if(enemiesInExplosionRange.Length > 0){
            foreach(Collider2D enemy in enemiesInExplosionRange){
                IDamageable iDamageble = enemy.GetComponent<IDamageable>();
                iDamageble?.TakeDamage(_grenadeDamage, 0);
            }
        }
        
        Instantiate(_explosionVFX, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void ScreenShake(){
        _impulseSource.GenerateImpulse();
    }
}