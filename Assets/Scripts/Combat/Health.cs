using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    public static Action<Health> OnDeath;

    public GameObject DeathSplatterVFX => _deathSplatterVFX;
    public GameObject DeathParticleVFX => _deathParticleVFX;
    public ColorChanger ColorChanger => _colorChanger;

    [SerializeField] private int _startingHealth = 3;
    [SerializeField] private GameObject _deathSplatterVFX;
    [SerializeField] private GameObject _deathParticleVFX;
    private int _currentHealth;
    private  ColorChanger _colorChanger;

    private void Awake() {
        _colorChanger = GetComponent<ColorChanger>();
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
}
