using System;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private Knockback _knockback;
    private float _moveX;
    [SerializeField] float _moveSpeed;
    private bool _canMove = true;

    private void Awake() {
        _rigidbody = GetComponent<Rigidbody2D>();
        _knockback = GetComponent<Knockback>();
    }

    private void OnEnable(){
        _knockback.OnKnockbackStart += CanMoveFalse;
        _knockback.OnKnockbackEnd += CanMoveTrue;
    }

    private void OnDisable(){
        _knockback.OnKnockbackStart -= CanMoveFalse;
        _knockback.OnKnockbackEnd -= CanMoveTrue;
    }

    private void FixedUpdate(){
        Move();
    }

    private void CanMoveFalse(){
        _canMove = false;
    }

    private void CanMoveTrue(){
        _canMove = true;
    }

    private void Move(){
        if(!_canMove){return;};

        _rigidbody.velocity = new Vector2(_moveX * _moveSpeed, _rigidbody.velocity.y);
    }

    public void SetMoveDirection(float direction){
        _moveX = direction;
    }
}
