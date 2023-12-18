using UnityEngine;

public class GeneralManager : MonoBehaviour
{
    public static GeneralManager Instance;

    public Transform EnemyManager => _enemyManager;
    public Transform BulletPoolManager => _bulletPoolManager;
    public Transform AudioSourceManager => _audioSourceManager;

    [SerializeField] private Transform _enemyManager;
    [SerializeField] private Transform _bulletPoolManager;
    [SerializeField] private Transform _audioSourceManager;

    private void Awake() {
        if(Instance == null) {Instance = this;}
    }
}