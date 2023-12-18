using UnityEngine;
using System.Collections;
using System;

public class DiscoBallManager : MonoBehaviour
{
    public static Action OnDiscoBallHitEvent;

    [SerializeField] private float _discoPatyTime = 2f;
    private ColorSpolight[] _allSpotLights;

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
        foreach(ColorSpolight spolight in _allSpotLights){
            spolight.DimTheLight(_discoPatyTime);
        }
    }
}
