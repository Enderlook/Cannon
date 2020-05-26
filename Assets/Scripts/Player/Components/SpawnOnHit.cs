using UnityEngine;

namespace Game.Ammunitions
{
    public class SpawnOnHit : MonoBehaviour
    {
        private Ammunition[] ammunitions;

        private bool hasAlreadSpawn = false;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by Unity.")]
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (hasAlreadSpawn)
                return;
            hasAlreadSpawn = true;
            for (int i = 0; i < ammunitions.Length; i++)
            {
                ContactPoint2D contactPoint2D = collision.GetContact(i % collision.contactCount);
                ammunitions[i].Shoot(contactPoint2D.normal * contactPoint2D.normalImpulse, contactPoint2D.point);
            }
        }

        public static void AddComponentTo(GameObject gameObject, Ammunition[] ammunitions)
        {
            SpawnOnHit component = gameObject.AddComponent<SpawnOnHit>();
            component.ammunitions = ammunitions;
        }
    }
}