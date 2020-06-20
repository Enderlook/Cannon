using Enderlook.Unity.Attributes;

using Game.Creatures;

using System;
using System.Linq;

using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Hurtable : MonoBehaviour, IDamagable
    {
#pragma warning disable CS0649
        [field: SerializeField, IsProperty, Tooltip("Score adquire when destroying this Game Object.")]
        public int Score { get; private set; }

        [SerializeField]
        private float health;
        public float Health {
            get => health;
            set {
                health = value;
                OnHealthChange?.Invoke(value);
            }
        }

        [SerializeField, Tooltip("Minimal amount of damage required to be hurt.")]
        private float damageThreshold = .1f;
#pragma warning restore CS0649

        private float maxHealth;

        private SpriteRenderer spriteRenderer;

        public event Action<float> OnHealthChange;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by Unity.")]
        protected virtual void Awake()
        {
            maxHealth = Health;
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by Unity.")]
        private void OnCollisionEnter2D(Collision2D collision)
        {
            float multiplier = 1;
            if (collision.otherCollider.TryGetComponent(out IDamager damager))
                multiplier = damager.DamageMultiplier;
            TakeDamage(Mathf.Abs(collision.contacts.Sum(e => e.normalImpulse)) * multiplier);
        }

        public virtual void TakeDamage(float amount)
        {
            amount -= damageThreshold;
            if (amount <= 0)
                return;
            Health = Mathf.Max(Health - amount, 0);
            spriteRenderer.color = Color.Lerp(Color.red, Color.white, Health / maxHealth);

            if (Health <= 0)
                Die();
        }

        public virtual void Die()
        {
            Array.ForEach(GetComponents<IDie>(), e => e.Die());
            Destroy(gameObject);
        }
    }
}