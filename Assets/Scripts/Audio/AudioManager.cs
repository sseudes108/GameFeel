using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour{   

    [Range(0, 2)]
    [SerializeField] private float _masterVolume = 1f;
    [SerializeField] private SoundCollectionSO _soundCollectionSO;
    [SerializeField] private AudioMixerGroup _musicMixerGroup, _SFXMixerGroup;
    private AudioSource _currentMusic;

#region Unity Methods
    private void OnEnable() {
        Gun.OnShot += Gun_OnShoot;

        PlayerController.OnJump += PlayerController_OnJump;
        PlayerController.OnJetPack += PlayerController_OnJetpack;

        Health.OnDeath += HandleDeath;
        
        DiscoBallManager.OnDiscoBallHitEvent += DiscoMusic;

        Grenade.OnGrenadeLaunch += Grenade_OnGrenadeLaunch;
        Grenade.OnGrenadeBeep += Grenade_OnGrenadeBeep;
        Grenade.OnGrenadeExplode += Grenade_OnGrenadeExplode;

        Enemy.OnPlayerHit += Enemy_OnPlayerHit;
    }

    private void OnDisable() {
        Gun.OnShot -= Gun_OnShoot;

        PlayerController.OnJump -= PlayerController_OnJump;
        PlayerController.OnJetPack -= PlayerController_OnJetpack;

        Health.OnDeath -= HandleDeath;

        DiscoBallManager.OnDiscoBallHitEvent -= DiscoMusic;

        Grenade.OnGrenadeLaunch -= Grenade_OnGrenadeLaunch;
        Grenade.OnGrenadeBeep -= Grenade_OnGrenadeBeep;
        Grenade.OnGrenadeExplode -= Grenade_OnGrenadeExplode;

        Enemy.OnPlayerHit -= Enemy_OnPlayerHit;
    }

    private void Start() {
        FightMusic();
    }
#endregion

#region Sound Methods
    private void PlayRandomSound(SoundSO[] sounds){
        if(sounds != null && sounds.Length > 0){
            SoundSO soundSO = sounds[Random.Range(0,sounds.Length)];
            SoundToPlay(soundSO);
        }
    }

    private void SoundToPlay(SoundSO soundSO) {
        AudioClip clip = soundSO.Clip;
        float pitch = soundSO.Pitch;
        float volume = soundSO.Volume * _masterVolume;
        bool loop = soundSO.Loop;
        AudioMixerGroup audioMixerGroup;

        pitch = RandomizePitch(soundSO, pitch);
        audioMixerGroup = DetermineAudioMixerGroup(soundSO);

        PlaySound(clip, pitch, volume, loop, audioMixerGroup);
    }

    private AudioMixerGroup DetermineAudioMixerGroup(SoundSO soundSO){
        AudioMixerGroup audioMixerGroup;
        switch (soundSO.AudioType){
            case SoundSO.AudioTypes.SFX:
                audioMixerGroup = _SFXMixerGroup;
                break;
            case SoundSO.AudioTypes.Music:
                audioMixerGroup = _musicMixerGroup;
                break;
            default:
                audioMixerGroup = null;
                break;
        }

        return audioMixerGroup;
    }

    private static float RandomizePitch(SoundSO soundSO, float pitch){
        if (soundSO.RandomizePitch){
            float randomPitchModifier = Random.Range(-soundSO.RandomPitchModifier, soundSO.RandomPitchModifier);
            pitch = soundSO.Pitch + randomPitchModifier;
        };
        return pitch;
    }

    private void PlaySound(AudioClip clip, float pitch, float volume, bool loop, AudioMixerGroup audioMixerGroup){
        GameObject soundObject = new GameObject("Temp Audio Source");
        AudioSource audioSource = soundObject.AddComponent<AudioSource>();
        soundObject.transform.SetParent(GeneralManager.Instance.AudioSourceManager);

        audioSource.clip = clip;
        audioSource.pitch = pitch;
        audioSource.volume = volume;
        audioSource.loop = loop;
        audioSource.outputAudioMixerGroup = audioMixerGroup;

        audioSource.Play();

        if (!loop) {Destroy(soundObject, clip.length);}

        DetermineMusic(audioMixerGroup, audioSource);
    }

    private void DetermineMusic(AudioMixerGroup audioMixerGroup, AudioSource audioSource){
        if (audioMixerGroup == _musicMixerGroup){
            if (_currentMusic != null)
            {
                _currentMusic.Stop();
            }
            _currentMusic = audioSource;
        }
    }
    #endregion

#region SFX
    private void Gun_OnShoot(){
        PlayRandomSound(_soundCollectionSO.ShotSounds);
    }

    private void Grenade_OnGrenadeLaunch(){
        PlayRandomSound(_soundCollectionSO.GrenadeShot);
    }
    private void Grenade_OnGrenadeBeep(){
        PlayRandomSound(_soundCollectionSO.GrenadeBeep);
    }
    private void Grenade_OnGrenadeExplode(){
        PlayRandomSound(_soundCollectionSO.GrenadeExplosion);
    }

    private void PlayerController_OnJump(){
        PlayRandomSound(_soundCollectionSO.JumpSounds);
    }
    private void PlayerController_OnJetpack(){
        PlayRandomSound(_soundCollectionSO.Jetpack);
    }

    private void Health_OnDeath(){
        PlayRandomSound(_soundCollectionSO.SplatSounds);
    }

    private void Enemy_OnPlayerHit(){
        PlayRandomSound(_soundCollectionSO.PlayerHit);
    }

    private void AudioManager_MegaKill(){
        PlayRandomSound(_soundCollectionSO.Megakill);
    }
#endregion

#region Music
    private void FightMusic(){
        PlayRandomSound(_soundCollectionSO.FightMusic);
    }

    private void DiscoMusic(){
        if(_currentMusic.clip.name == _soundCollectionSO.DiscoMusic.name){return;}

        SoundToPlay(_soundCollectionSO.DiscoMusic);
        float soundLenght;
        soundLenght = _soundCollectionSO.DiscoMusic.Clip.length;
        Utils.RunAfterDelay(this, soundLenght, FightMusic);
    }
#endregion

#region Custom SFX logic

private List<Health> _deathList = new List<Health>();
private Coroutine _deathRoutine;

private void HandleDeath(Health health){
    bool isEnemy = health.GetComponent<Enemy>();

    if(isEnemy){
        _deathList.Add(health);
    }

    if(_deathRoutine == null){
        _deathRoutine = StartCoroutine(DeathWindowRoutine());
    }
}

private IEnumerator DeathWindowRoutine(){
    yield return null;

    int megakill = 3;

    if(_deathList.Count >= megakill){
        AudioManager_MegaKill();
    }

    Health_OnDeath();

    _deathList.Clear();
    _deathRoutine = null;
}

#endregion

}