using UnityEngine;

namespace Game.Ammunitions
{
    [CreateAssetMenu(fileName = "Projectile Destroyer", menuName = "Game/Cannon/Attach/Projectile Destroyer")]
    public class ProjectileDestroyer : Attach
    {
#pragma warning disable CS0649
        [SerializeField, Tooltip("Time in seconds to destroy projetile if nothing happens.")]
        private float destroyTime = 60;

        [SerializeField, Tooltip("Time in seconds to destroy projetile after impact.")]
        private float destroyTimeAfterImpact = 5;
#pragma warning restore CS0649

        public override void Accept(GameObject gameObject)
        {
            Destroy(gameObject, destroyTime);
            DestroyAfterCollision.AddComponentTo(gameObject, destroyTimeAfterImpact);
        }
    }
}