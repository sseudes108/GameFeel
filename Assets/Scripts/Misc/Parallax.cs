using UnityEngine;

public class Parallax : MonoBehaviour{
    [SerializeField] private float _parallaxOffset;
    private Camera _mainCamera;
    private Vector2 _startPos;
    private Vector2 _travel => (Vector2)_mainCamera.transform.position - _startPos;
    private void Awake() {
        _mainCamera = Camera.main;
    }
    private void Start() {
        _startPos = transform.position;
    }

    private void FixedUpdate() {
        Vector2 newPosition = _startPos + new Vector2(_travel.x * _parallaxOffset, 0f);
        transform.position = new Vector2(newPosition.x, transform.position.y);
    }
}
