using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Cinemachine;

public class Grenade : MonoBehaviour {

    public static Action OnGrenadeLaunch, OnGrenadeBeep, OnGrenadeExplode;

    private Rigidbody2D _rigidbody;
    private Vector2 _fireDirection;
    private Vector2 _mousePos;
    private Vector2 _spawnPosition;

    [Header("Grenade")]
    [SerializeField] private float _launchForce;
    [SerializeField] private float _torqueAmount;
    [SerializeField] private int _grenadeDamage = 3;
    [SerializeField] private float _explosionRange;
    [SerializeField] private GameObject _explosionVFX;
    [SerializeField] private LayerMask _enemyLayerMask;

    [Header("Lights")]
    [SerializeField] float _flashLightTime = 0.5f;
    private Light2D _light2D;
    
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
        _rigidbody.AddTorque(_torqueAmount, ForceMode2D.Impulse);
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
        CheckEnemiesInRange();
        ScreenShake();

        Instantiate(_explosionVFX, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void CheckEnemiesInRange(){
        Collider2D[] enemiesInExplosionRange = Physics2D.OverlapCircleAll(transform.position, _explosionRange, _enemyLayerMask);
        if (enemiesInExplosionRange.Length > 0){
            foreach (Collider2D enemy in enemiesInExplosionRange){
                IDamageable iDamageble = enemy.GetComponent<IDamageable>();
                iDamageble?.TakeDamage(transform.position, _grenadeDamage, 0);
            }
        }
    }

    private void ScreenShake(){
        _impulseSource.GenerateImpulse();
    }
}