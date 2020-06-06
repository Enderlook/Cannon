using UnityEngine;

namespace Game.Ammunitions
{
    [CreateAssetMenu(fileName = "Explode On Hit", menuName = "Game/Cannon/Attach/Explode On Hit")]
    public class ExplodeOnHitAttach : Attach
    {
#pragma warning disable CS0649
        [SerializeField, Tooltip("Damage done over distance.")]
        private float damageOverDistance = 1;

        [SerializeField, Tooltip("Force applied on explosion.")]
        private float explosionForce = 1;

        [SerializeField, Tooltip("Minimal amount of damage required to produce it.")]
        private float damageThreshold = .25f;
#pragma warning restore CS0649

        public override void Accept(GameObject gameObject) => ExplodeOnHit.AddComponentTo(gameObject, explosionForce, damageOverDistance, damageThreshold);
    }
}
