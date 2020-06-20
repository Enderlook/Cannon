using Enderlook.Unity.Attributes;

using UnityEngine;

namespace Game.Ammunitions
{
    [CreateAssetMenu(fileName = "Base Ammunition", menuName = "Game/Cannon/Ammunition/Base Ammunition")]
    public class Ammunition : ScriptableObject, IAmmunition
    {
#pragma warning disable CS0649
        [SerializeField, Tooltip("Layer of projectile."), Layer]
        private int layer;

        [SerializeField, Min(.1f), Tooltip("Multiplies shooting force from the cannon.")]
        private float forceMultiplier = 1;

        [SerializeField, Min(.1f), Tooltip("Mass of projectile.")]
        private float mass = 1;

        [SerializeField, Min(.1f), Tooltip("Scale of projectile.")]
        private float scale = 1;

        [field: SerializeField, IsProperty, Tooltip("Sprite of projectile.")]
        public Sprite Sprite { get; private set; }

        [SerializeField, Tooltip("Additional settings added to the generated projectile.")]
        private Attach[] attachs;

        [field: SerializeField, IsProperty]
        public int Score { get; private set; }
#pragma warning restore CS0649

        public void Shoot(Vector3 force, Vector3 position)
        {
            GameObject projectile = CreateProjectile(force, position);
            foreach (Attach attach in attachs)
                attach.Accept(projectile);
        }

        protected virtual GameObject CreateProjectile(Vector3 force, Vector3 position)
            => SpawnProjectile(force * forceMultiplier, position, mass, scale, Sprite);

        public GameObject SpawnProjectile(Vector3 force, Vector3 position, float mass, float scale, Sprite sprite)
        {
            GameObject gameObject = new GameObject("Projectile")
            {
                layer = layer
            };
            Transform transform = gameObject.transform;
            transform.position = position;
            transform.localScale = Vector3.one * scale;

            Rigidbody2D rigidbody = gameObject.AddComponent<Rigidbody2D>();
            rigidbody.mass = mass;
            rigidbody.AddForce(force, ForceMode2D.Impulse);

            SpriteRenderer spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = sprite;

            gameObject.AddComponent<CircleCollider2D>();

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
            } while (Physics2D.OverlapCircle(positions[i], scale) == null && i++ < positions.Length - 1);

            return i;
        }
    }
}
