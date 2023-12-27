using System;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
    public static Action<Health> OnDeath;

    public GameObject DeathSplatterVFX => _deathSplatterVFX;
    public GameObject DeathParticleVFX => _deathParticleVFX;

    [SerializeField] private int _startingHealth = 3;
    [SerializeField] private GameObject _deathSplatterVFX;
    [SerializeField] private GameObject _deathParticleVFX;
    private int _currentHealth;

    private Knockback _knockback;
    private Flash _flash;
    private Health _health;

    private void Awake() {
        _flash = GetComponent<Flash>();
        _health = GetComponent<Health>();
        _knockback = GetComponent<Knockback>();
    }

    private void Start() {
        ResetHealth();
    }

    public void ResetHealth() {
        _currentHealth = _startingHealth;
    }

    public void TakeDamage(int amount) {
        _currentHealth -= amount;

        if (_currentHealth <= 0) {
            OnDeath?.Invoke(this);
            Destroy(gameObject);
        }
    }

    public void TakeHit(){
       _flash.StartFlash();
    }

    public void TakeDamage(Vector2 damageSourceDir, int damageAmount, float knockbackThrust){
        _health.TakeDamage(damageAmount);
        _knockback.GetKnockedBack(damageSourceDir, knockbackThrust);
    }
}
