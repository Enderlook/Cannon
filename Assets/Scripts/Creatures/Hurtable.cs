using Game.Creatures;

using System;
using System.Linq;

using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Hurtable : MonoBehaviour, IDamagable
    {
        [SerializeField]
        private float health;
        public float Health {
            get => health;
            set {
                health = value;
                OnHealthChange(value);
            }
        }

        [SerializeField, Tooltip("Minimal amount of damage required to be hurt.")]
        private float damageThreshold = .1f;

        [SerializeField]
        private string hurtAnimation;

        [SerializeField]
        private string deathAnimation;

        private float maxHealth;

        private SpriteRenderer spriteRenderer;
        private Animator animator;

        public event Action<float> OnHealthChange;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by Unity.")]
        private void Awake()
        {
            maxHealth = Health;
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by Unity.")]
        private void OnCollisionEnter2D(Collision2D collision)
            => TakeDamage(Mathf.Abs(collision.contacts.Sum(e => e.normalImpulse) * (collision.otherCollider.GetComponent<IDamager>()?.DamageMultiplier ?? 1)));

        public void TakeDamage(float amount)
        {
            amount -= damageThreshold;
            if (amount <= 0)
                return;
            Health = Mathf.Max(Health - amount, 0);
            spriteRenderer.color = Color.Lerp(Color.red, Color.white, Health / maxHealth);

            if (Health > 0)
            {
                if (animator != null && !string.IsNullOrEmpty(hurtAnimation))
                    animator.Play(hurtAnimation);
            }
            else
                Kill();
        }

        public void Kill()
        {
            if (animator != null && !string.IsNullOrEmpty(deathAnimation))
                animator.Play(deathAnimation);
            else
                Die();
        }

        private void Die()
        {
            Array.ForEach(GetComponents<IDie>(), e => e.Die());
            Destroy(gameObject);
        }
    }
}