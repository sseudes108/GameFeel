using UnityEngine;
using TMPro;

public class Score : MonoBehaviour{
    private TMP_Text _scoretext;
    private int _score = 0;

    private void Awake() {
        _scoretext = GetComponent<TMP_Text>();
    }

    private void OnEnable() {
        Health.OnDeath += UpdateScore;
    }
    private void OnDisable() {
        Health.OnDeath -= UpdateScore;
    }

    private void UpdateScore(Health sender){
        Enemy enemy = sender.GetComponent<Enemy>();

        if(enemy){
            _score ++;
            _scoretext.text = _score.ToString("D3");
        }
    }
}
