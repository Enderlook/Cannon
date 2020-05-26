using System.Linq;

using UnityEngine;

namespace Game.Ammunitions
{
    public class HurtOnHit : MonoBehaviour
    {
        private float damageMultiplier = 1;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by Unity.")]
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.otherCollider.TryGetComponent(out IDamagable damagable))
                damagable.TakeDamage(collision.contacts.Sum(e => e.normalImpulse) * damageMultiplier);
        }

        public static void AddComponentTo(GameObject gameObject, float damageMultiplier)
        {
            HurtOnHit component = gameObject.AddComponent<HurtOnHit>();
            component.damageMultiplier = damageMultiplier;
        }
    }
}