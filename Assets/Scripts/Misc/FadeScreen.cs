using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using System.Collections;

public class FadeScreen : MonoBehaviour{
    [SerializeField] private float _fadeTime;
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private Transform _respawnPoint;

    private Image _fadeScreen;
    private CinemachineVirtualCamera _virtualCam;
    
    private void Awake() {
        _fadeScreen = GetComponent<Image>();
        _virtualCam = FindFirstObjectByType<CinemachineVirtualCamera>();
    }

    public void FadeInAndOut(){
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn(){
        yield return StartCoroutine(FadeRoutine(1f));
        RespawnPlayer();
        StartCoroutine(FadeRoutine(0));
    }

    private void RespawnPlayer(){
        GameObject player = Instantiate(_playerPrefab, _respawnPoint.position, Quaternion.identity);
        _virtualCam.Follow = player.transform;
    }

    private IEnumerator FadeRoutine(float targetAlpha){
        float elapsedTime = 0f;
        float startAlpha = _fadeScreen.color.a;

        while (elapsedTime < _fadeTime){
            elapsedTime += Time.deltaTime;

            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / _fadeTime);
            _fadeScreen.color = new Color(_fadeScreen.color.r, _fadeScreen.color.g, _fadeScreen.color.b, newAlpha);
            yield return null;
        }
        
        _fadeScreen.color = new Color(_fadeScreen.color.r, _fadeScreen.color.g, _fadeScreen.color.b, targetAlpha);
    }
}