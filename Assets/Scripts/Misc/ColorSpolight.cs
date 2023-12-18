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

    public void DimTheLight(float discoPatyTime){
        if(_dimLightsRoutine != null){
            StopCoroutine(_dimLightsRoutine);
        }
        _dimLightsRoutine = StartCoroutine(DimTheLightsRoutine(discoPatyTime));
    }

    private IEnumerator DimTheLightsRoutine(float discoPatyTime){
        Debug.Log(string.Format("Rotation speed at start: {0}", _defaulRotationSpeed));

        _rotationSpeed += Random.Range(80, 160);
        Debug.Log(string.Format("New rotation speed: {0}", _rotationSpeed));

        yield return new WaitForSeconds(discoPatyTime);

        _rotationSpeed = _defaulRotationSpeed;
        Debug.Log(string.Format("Rotation speed at end: {0}", _rotationSpeed));
    }
}
