using UnityEngine;

[CreateAssetMenu()]
public class SoundCollectionSO : ScriptableObject {
    
    public SoundSO[] ShotSounds => _shot;
    public SoundSO[] JumpSounds => _jump;
    public SoundSO[] SplatSounds => _splat;
    public SoundSO[] FightMusic => _fightMusic;
    public SoundSO DiscoMusic => _discoMusic;

    [Header("SFX")]
    [SerializeField] private SoundSO[] _shot;
    [SerializeField] private SoundSO[] _jump;
    [SerializeField] private SoundSO[] _splat;

    [Header("Music")]
    [SerializeField] private SoundSO[] _fightMusic;
    [SerializeField] private SoundSO _discoMusic;
}
