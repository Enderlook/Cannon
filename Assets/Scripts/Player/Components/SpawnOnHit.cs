using UnityEngine;

namespace Game.Ammunitions
{
    public class SpawnOnHit : MonoBehaviour
    {
        private Ammunition[] ammunitions;

        private float randomization;

        private bool hasAlreadySpawn = false;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by Unity.")]
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (hasAlreadySpawn)
                return;
            hasAlreadySpawn = true;
            for (int i = 0; i < ammunitions.Length; i++)
            {
                ContactPoint2D contactPoint2D = collision.GetContact(i % collision.contactCount);
                ammunitions[i].Shoot(contactPoint2D.normal * contactPoint2D.normalImpulse * ((Vector2.one * (1 - randomization)) + ((Vector2)Random.onUnitSphere) * randomization), contactPoint2D.point);
            }
        }

        public static void AddComponentTo(GameObject gameObject, Ammunition[] ammunitions, float randomization)
        {
            SpawnOnHit component = gameObject.AddComponent<SpawnOnHit>();
            component.ammunitions = ammunitions;
            component.randomization = randomization;
        }
    }
}