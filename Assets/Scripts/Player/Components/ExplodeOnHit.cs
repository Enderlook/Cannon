using UnityEngine;

namespace Game.Ammunitions
{
    public class ExplodeOnHit : MonoBehaviour
    {
        private float damageOverDistance;

        private float explosionForce;

        private float damageThreshold;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by Unity.")]
        private void OnCollisionEnter2D(Collision2D collision)
        {
            foreach (Collider2D collider in Physics2D.OverlapCircleAll(transform.position, 5))
            {
                if (collider.gameObject == gameObject)
                    continue;

                Vector3 direction = (collider.transform.position - transform.position).normalized;
                if (collider.TryGetComponent(out Rigidbody2D rigidbody2D))
                    rigidbody2D.AddForceAtPosition(direction * explosionForce, transform.position);
                float damage = damageOverDistance / direction.magnitude;
                if (damage > damageThreshold)
                {
                    if (collider.TryGetComponent(out IDamagable damagable))
                        damagable.TakeDamage(damageOverDistance / direction.magnitude);
                    if (collider.TryGetComponent(out SoundOnHit soundOnHit))
                        soundOnHit.PlaySound();
                }
            }

            Destroy(gameObject);
        }

        public static void AddComponentTo(GameObject gameObject, float explosionForce, float damageOverDistance, float damageThreshold)
        {
            ExplodeOnHit component = gameObject.AddComponent<ExplodeOnHit>();
            component.explosionForce = explosionForce;
            component.damageOverDistance = damageOverDistance;
            component.damageThreshold = damageThreshold;
        }
    }
}