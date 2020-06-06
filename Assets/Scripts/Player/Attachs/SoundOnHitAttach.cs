using UnityEngine;
using UnityEngine.Audio;

namespace Game.Ammunitions
{
    [CreateAssetMenu(fileName = "Sound On Hit", menuName = "Game/Cannon/Attach/Sound On Hit")]
    public class SoundOnHitAttach : Attach
    {
#pragma warning disable CS0649
        [SerializeField, Tooltip("Sound played on hit.")]
        private AudioClip audioClip;

        [SerializeField, Tooltip("Audio Mixer Group used to play sound.")]
        private AudioMixerGroup audioMixerGroup;
#pragma warning restore CS0649

        public override void Accept(GameObject gameObject) => SoundOnHit.AddComponentTo(gameObject, audioClip, audioMixerGroup);
    }
}
