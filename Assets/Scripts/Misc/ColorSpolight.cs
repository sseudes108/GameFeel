using UnityEngine;
using System.Collections;

public class ColorSpolight : MonoBehaviour
{
    [SerializeField] GameObject _spotlightHead;
    [SerializeField] private float _rotationSpeed = 20f;
    private float _defaulRotationSpeed;
    [SerializeField] private float _maxRotation = 45f;
    private float _currentRotation;

    private Coroutine _dimLightsRoutine;

    private void Start() {
        RandomStartingRotation();
        _defaulRotationSpeed = _rotationSpeed;
    }

    private void Update() {
        RotateHead();
    }

    private void RotateHead(){
        _currentRotation += Time.deltaTime * _rotationSpeed;
        float z = Mathf.PingPong(_currentRotation, _maxRotation);
        _spotlightHead.transform.localRotation = Quaternion.Euler(0f, 0f, z);
    }

    private void RandomStartingRotation(){
        _currentRotation = Random.Range(- _maxRotation, _maxRotation);
    }

    public void SetRotationSpeed(float newSpeed){
        _rotationSpeed = newSpeed;
    }
    public void ResetRotationSpeed(){
        _rotationSpeed = _defaulRotationSpeed;
    }

    public void SpotLightDiscoParty(float discoPatyTime){
        if(_dimLightsRoutine != null){
            StopCoroutine(_dimLightsRoutine);
            _rotationSpeed = _defaulRotationSpeed;
        }
        _dimLightsRoutine = StartCoroutine(DiscoPatyLightsRoutine(discoPatyTime));
    }

    private IEnumerator DiscoPatyLightsRoutine(float discoPatyTime){
        
        _rotationSpeed += Random.Range(80, 160);
        
        yield return new WaitForSeconds(discoPatyTime);

        _rotationSpeed = _defaulRotationSpeed;
        _dimLightsRoutine = null;
    }
}
