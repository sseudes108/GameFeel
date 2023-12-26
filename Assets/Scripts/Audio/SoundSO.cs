using UnityEngine;

[CreateAssetMenu()]
public class SoundSO : ScriptableObject
{
    public enum AudioTypes{
        SFX,
        Music
    }

    public AudioTypes AudioType;
    public AudioClip Clip;

    public bool Loop;
    public bool RandomizePitch;

    [Range(0f, 1f)]
    public float RandomPitchModifier = 0.1f;

    [Range(0.1f, 4f)]
    public float Volume = 1f;

    [Range(0.1f, 3f)]
    public float Pitch = 1f;
}
