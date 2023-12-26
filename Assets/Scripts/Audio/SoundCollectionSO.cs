using UnityEngine;

[CreateAssetMenu()]
public class SoundCollectionSO : ScriptableObject {
    
    public SoundSO[] ShotSounds => _shot;
    public SoundSO[] JumpSounds => _jump;
    public SoundSO[] SplatSounds => _splat;
    public SoundSO[] FightMusic => _fightMusic;
    public SoundSO DiscoMusic => _discoMusic;
    public SoundSO Jetpack => _jetpack;
    public SoundSO GrenadeBeep => _grenadeBeep;
    public SoundSO GrenadeShot => _grenadeShot;
    public SoundSO GrenadeExplosion => _grenadeExplosion;

    [Header("SFX")]
    [SerializeField] private SoundSO[] _shot;
    [SerializeField] private SoundSO[] _jump;
    [SerializeField] private SoundSO[] _splat;
    [SerializeField] private SoundSO _jetpack;
    [SerializeField] private SoundSO _grenadeBeep;
    [SerializeField] private SoundSO _grenadeShot;
    [SerializeField] private SoundSO _grenadeExplosion;

    [Header("Music")]
    [SerializeField] private SoundSO[] _fightMusic;
    [SerializeField] private SoundSO _discoMusic;
}
