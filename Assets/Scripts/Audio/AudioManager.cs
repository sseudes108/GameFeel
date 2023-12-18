using UnityEngine;
using UnityEngine.Pool;

public class AudioManager : MonoBehaviour
{    
    [SerializeField] private float _masterVolume = 1f;
    [SerializeField] private SoundSO _gunShoot;
    [SerializeField] private SoundSO _playerJump;

    private void OnEnable() {
        Gun.OnShot += Gun_OnShoot;
        PlayerController.OnJump += PlayerController_OnJump;
    }

    private void OnDisable() {
        Gun.OnShot -= Gun_OnShoot;
        PlayerController.OnJump -= PlayerController_OnJump;
    }

    private void SoundToPlay(SoundSO soundSO){
        AudioClip clip = soundSO.Clip;
        float pitch = soundSO.Pitch;
        float volume = soundSO.Volume  * _masterVolume;
        bool loop = soundSO.Loop;

        if(soundSO.RandomizePitch){
            float randomPitchModifier = UnityEngine.Random.Range(-soundSO.RandomPitchModifier, soundSO.RandomPitchModifier);
            pitch = soundSO.Pitch + randomPitchModifier;
        };

        PlaySound(clip, pitch, volume, loop);
    }

    private void PlaySound(AudioClip clip, float pitch, float volume, bool loop){
        GameObject soundObject = new GameObject("Temp Audio Source");
        AudioSource audioSource = soundObject.AddComponent<AudioSource>();
        soundObject.transform.SetParent(GeneralManager.Instance.AudioSourceManager);

        audioSource.clip = clip;
        audioSource.pitch = pitch;
        audioSource.volume = volume;
        audioSource.loop = loop;

        audioSource.Play();

        if(!loop){Destroy(soundObject, clip.length);}
    }

    private void Gun_OnShoot(){
        SoundToPlay(_gunShoot);
    }

    private void PlayerController_OnJump(){
        SoundToPlay(_playerJump);
    }

}
