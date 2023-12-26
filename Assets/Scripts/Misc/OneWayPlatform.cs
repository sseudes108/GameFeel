using System.Collections;
using UnityEngine;

public class OneWayPlatform : MonoBehaviour{
    private Collider2D _collider;
    private bool _playerIsOnPlatform;

    private void Awake() {
        _collider = GetComponent<Collider2D>();
    }

    private void Update() {
        DetectPlayerInput();
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.GetComponent<PlayerController>()){
            _playerIsOnPlatform = true;
        }
    }

    private void OnCollisionExit(Collision other) {
        if(other.gameObject.GetComponent<PlayerController>()){
            _playerIsOnPlatform = false;
        }
    }

    private void DetectPlayerInput(){
        if(!_playerIsOnPlatform) return;

        if(PlayerController.Instance.FrameInput.Move.y < 0f){
            StartCoroutine(DisableCollidersRoutine());
        }
    }

    private IEnumerator DisableCollidersRoutine(){
        Collider2D[] colliders = PlayerController.Instance.GetComponents<Collider2D>();

        foreach(Collider2D collider in colliders){
            Physics2D.IgnoreCollision(collider, _collider, true);
        }

        yield return new WaitForSeconds(1);

        foreach(Collider2D collider in colliders){
            Physics2D.IgnoreCollision(collider, _collider, false);
        }
    }

}
