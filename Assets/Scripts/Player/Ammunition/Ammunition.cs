using Enderlook.Unity.Attributes;

using UnityEngine;

namespace Game.Ammunitions
{
    [CreateAssetMenu(fileName = "Base Ammunition", menuName = "Game/Ammunition/Base Ammunition")]
    public class Ammunition : ScriptableObject, IAmmunition
    {
#pragma warning disable CS0649
        [SerializeField, Min(.1f), Tooltip("Multiplies shooting force from the cannon.")]
        private float forceMultiplier = 1;

        [SerializeField, Min(.1f), Tooltip("Mass of projectile.")]
        private float mass = 1;

        [SerializeField, Min(.1f), Tooltip("Damage multiplier done at impact.")]
        private float damage = 1;

        [SerializeField, Min(.1f), Tooltip("Radius of circule collider.")]
        private float colliderRadius = 1;

        [field: SerializeField, IsProperty, Tooltip("Sprite of projectile.")]
        public Sprite Sprite { get; private set; }
#pragma warning restore CS0649

        public void Shoot(Vector3 force, Vector3 position) => CreateProjectile(force, position);

        protected virtual GameObject CreateProjectile(Vector3 force, Vector3 position)
        {
            GameObject gameObject = new GameObject("Projectile");
            Transform transform = gameObject.transform;
            transform.position = position;

            Rigidbody2D rigidbody = gameObject.AddComponent<Rigidbody2D>();
            rigidbody.mass = mass;
            rigidbody.AddForce(force * forceMultiplier, ForceMode2D.Impulse);

            CircleCollider2D collider = gameObject.AddComponent<CircleCollider2D>();
            collider.radius = colliderRadius;

            SpriteRenderer spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = Sprite;
            spriteRenderer.size = Vector2.one * colliderRadius * 2;

            return gameObject;
        }

        public virtual int GetPredictedPositions(Vector3 force, Vector3 startPosition, Vector2[] positions, float timeScale)
        {
            force = force * forceMultiplier / mass;

            int i = 0;
            do
            {
                float t = i * timeScale;

                float x = startPosition.x + (force.x * t);
                float y = startPosition.y + (force.y * t) + (.5f * Physics2D.gravity.y * t * t);

                positions[i] = new Vector2(x, y);
            } while (Physics2D.OverlapCircle(positions[i], colliderRadius) == null && i++ < positions.Length - 1);

            return i;
        }
    }
}
