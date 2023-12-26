using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour{   
    public static AudioManager Instance;

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
        Health.OnDeath += Health_OnDeath;
        DiscoBallManager.OnDiscoBallHitEvent += DiscoMusic;

        Grenade.OnGrenadeLaunch += Grenade_OnGrenadeLaunch;
        Grenade.OnGrenadeBeep += Grenade_OnGrenadeBeep;
        Grenade.OnGrenadeExplode += Grenade_OnGrenadeExplode;
    }

    private void OnDisable() {
        Gun.OnShot -= Gun_OnShoot;
        PlayerController.OnJump -= PlayerController_OnJump;
        PlayerController.OnJetPack -= PlayerController_OnJetpack;
        Health.OnDeath -= Health_OnDeath;
        DiscoBallManager.OnDiscoBallHitEvent -= DiscoMusic;

        Grenade.OnGrenadeLaunch -= Grenade_OnGrenadeLaunch;
        Grenade.OnGrenadeBeep -= Grenade_OnGrenadeBeep;
        Grenade.OnGrenadeExplode -= Grenade_OnGrenadeExplode;
        
    }
    private void Awake() {
        if(Instance == null){Instance = this;}
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

#region SFX Events
    private void Gun_OnShoot(){
        PlayRandomSound(_soundCollectionSO.ShotSounds);
    }

    private void Grenade_OnGrenadeLaunch(){
        SoundToPlay(_soundCollectionSO.GrenadeShot);
    }
    private void Grenade_OnGrenadeBeep(){
        SoundToPlay(_soundCollectionSO.GrenadeBeep);
    }
    private void Grenade_OnGrenadeExplode(){
        SoundToPlay(_soundCollectionSO.GrenadeExplosion);
    }

    private void PlayerController_OnJump(){
        PlayRandomSound(_soundCollectionSO.JumpSounds);
    }
    private void PlayerController_OnJetpack(){
        SoundToPlay(_soundCollectionSO.Jetpack);
    }

    private void Health_OnDeath(Health health){
        PlayRandomSound(_soundCollectionSO.SplatSounds);
    }
#endregion

#region Music Events
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

}