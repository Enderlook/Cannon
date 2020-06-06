using Enderlook.Unity.Components.Destroy;

using System.Linq;

using UnityEngine;
using UnityEngine.Audio;

namespace Game.Ammunitions
{
    [AddComponentMenu("Game/Sound On Hit")]
    public class SoundOnHit : MonoBehaviour
    {
#pragma warning disable CS0649
        [SerializeField, Tooltip("Sound played on collision.")]
        private AudioClip audioClip;

        [SerializeField, Tooltip("Audio Mixer Group used to play sound.")]
        private AudioMixerGroup audioMixerGroup;

        [SerializeField, Tooltip("Minimal amount of force required to play sound.")]
        private float forceThreshold = 0;
#pragma warning restore CS0649

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by Unity.")]
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (forceThreshold != 0 && Mathf.Abs(collision.contacts.Sum(e => e.normalImpulse)) < forceThreshold)
                return;

            PlaySound();
        }

        public void PlaySound()
        {
            GameObject sound = new GameObject("Sound On Hit");
            AudioSource audioSource = sound.AddComponent<AudioSource>();
            audioSource.outputAudioMixerGroup = audioMixerGroup;
            audioSource.clip = audioClip;
            audioSource.pitch = Random.Range(.8f, 1.2f);
            audioSource.volume = Random.Range(.8f, 1f);
            audioSource.Play();
            DestroyWhenAudioSourceEnds.AddComponent(sound);
        }

        public static void AddComponentTo(GameObject gameObject, AudioClip audioClip, AudioMixerGroup audioMixerGroup)
        {
            SoundOnHit component = gameObject.AddComponent<SoundOnHit>();
            component.audioClip = audioClip;
            component.audioMixerGroup = audioMixerGroup;
        }
    }
}