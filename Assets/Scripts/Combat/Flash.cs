using System.Collections;
using UnityEngine;

public class Flash : MonoBehaviour
{
    [SerializeField] private Material _defaulMaterial, _whiteMaterial;
    [SerializeField] private float _flashTime = 0.1f;
    [SerializeField] private SpriteRenderer[] _spriteRenderers;
    private ColorChanger _colorChanger;

    private void Awake() {
        _spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        _colorChanger = GetComponent<ColorChanger>();
    }

    public void StartFlash(){
        StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine(){
        foreach (SpriteRenderer spriteRenderer in _spriteRenderers){
            spriteRenderer.material = _whiteMaterial;
            _colorChanger?.SetColor(Color.white);

            yield return new WaitForSeconds(_flashTime);
        }
        SetDefaultMaterial();
        _colorChanger?.SetColor(_colorChanger.DefaultColor);
    }

    private void SetDefaultMaterial(){
        foreach (SpriteRenderer spriteRenderer in _spriteRenderers){
            spriteRenderer.material = _defaulMaterial;
        }
    }
}
