using System;
using System.Collections;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    public Action OnKnockbackStart, OnKnockbackEnd; 
    [SerializeField] private float _knockbacktime = 0.2f;

    private Vector3 _hitDirection;
    private float _knockbackThrust;
    private Rigidbody2D _rigidbody;
    private void Awake() {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnEnable() {
        OnKnockbackStart += ApplyKnockbackForce;
        OnKnockbackEnd += StopKnockbackRoutine;
    }
    private void OnDisable() {
        OnKnockbackStart -= ApplyKnockbackForce;
        OnKnockbackEnd -= StopKnockbackRoutine;
    }

    public void GetKnockedBack(Vector3 hitDirection, float knockbackThrust){
        _hitDirection = hitDirection;
        _knockbackThrust = knockbackThrust;

        OnKnockbackStart?.Invoke();
    }

    private void ApplyKnockbackForce(){
        Vector3 difference = (transform.position - _hitDirection).normalized * _knockbackThrust * _rigidbody.mass;
        _rigidbody.AddForce(difference, ForceMode2D.Impulse);
        StartCoroutine(KnockRoutine());
    }

    private IEnumerator KnockRoutine(){
        yield return new WaitForSeconds(_knockbacktime);
        OnKnockbackEnd?.Invoke();
    }

    private void StopKnockbackRoutine(){
        _rigidbody.velocity = Vector2.zero;
    }

}
