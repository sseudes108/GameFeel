using UnityEngine;

public class ColorChanger : MonoBehaviour
{  
    public Color DefaultColor {get; private set;}
    [SerializeField] SpriteRenderer _fillSpriteRenderer;
    [SerializeField] Color[] _colors;

    public void SetDefaultColor(Color color){
        DefaultColor = color;
        SetColor(color);
    }

    public void SetColor(Color color){
        _fillSpriteRenderer.color = color;
    }

    public void SetRandomColor(){
        int randomColor = Random.Range(0, _colors.Length);
        DefaultColor = _colors[randomColor];
        _fillSpriteRenderer.color = DefaultColor;
    }
}
