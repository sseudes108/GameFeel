using UnityEngine;
using System;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class DiscoBallManager : MonoBehaviour
{
    public static Action OnDiscoBallHitEvent;

    [SerializeField] private Light2D _globalLight;
    [SerializeField] private float _discoGlobalLightIntensity = 0.2f;
    private float _defaulGlobalIntensity;
    private Coroutine _globalLightRoutine;

    //Spot Lights
    [SerializeField] private float _discoPartyTime = 7f;
    private ColorSpolight[] _allSpotLights;

    private void Awake() {
        _defaulGlobalIntensity = _globalLight.intensity;
    }

    void Start(){
        _allSpotLights = FindObjectsByType<ColorSpolight>(FindObjectsSortMode.None);
    }

    private void OnEnable(){
        OnDiscoBallHitEvent += DimTheLights;
    }
    private void OnDisable() {
        OnDiscoBallHitEvent -= DimTheLights;
    }

    private void DimTheLights(){
        DiscoGlobalLight();

        foreach(ColorSpolight spolight in _allSpotLights){
            spolight.SpotLightDiscoParty(_discoPartyTime);
        }
    }

    public void DiscoGlobalLight(){
        if(_globalLightRoutine != null){
            StopCoroutine(_globalLightRoutine);
        }
        _globalLightRoutine = StartCoroutine(GlobalLightResetRoutine());
    }

    private IEnumerator GlobalLightResetRoutine(){
        _globalLight.intensity = _discoGlobalLightIntensity;

        yield return new WaitForSeconds(_discoPartyTime);

        _globalLight.intensity = _defaulGlobalIntensity;

        _globalLightRoutine = null;
    }
}
