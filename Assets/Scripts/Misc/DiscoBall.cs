using UnityEngine;

public class DiscoBall : MonoBehaviour, IHitable
{
    private Flash _flash;

    private void Awake() {
        _flash = GetComponent<Flash>();
    }

    public void TakeHit(){
        _flash.StartFlash();
        DiscoBallManager.OnDiscoBallHitEvent?.Invoke();
    }
}