using Enderlook.Unity.Components.Destroy;

using UnityEngine;
using UnityEngine.Audio;

namespace Game.Ammunitions
{
    public class SoundOnHit : MonoBehaviour
    {
        private AudioClip audioClip;
        private AudioMixerGroup audioMixerGroup;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by Unity.")]
        private void OnCollisionEnter2D(Collision2D collision)
        {
            GameObject sound = new GameObject();
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