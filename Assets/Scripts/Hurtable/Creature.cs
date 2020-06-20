using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Animator))]
    public class Creature : Hurtable
    {
        [SerializeField]
        private string hurtAnimation;

        [SerializeField]
        private string deathAnimation;

        private Animator animator;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by Unity.")]
        protected override void Awake()
        {
            base.Awake();
            animator = GetComponent<Animator>();
        }

        public override void TakeDamage(float amount)
        {
            base.TakeDamage(amount);
            if (Health > 0 && animator != null && !string.IsNullOrEmpty(hurtAnimation))
                animator.Play(hurtAnimation);
        }

        public override void Die()
        {
            if (animator != null && !string.IsNullOrEmpty(deathAnimation))
                animator.Play(deathAnimation);
            else
                Destroy();
        }

        public void Destroy() => base.Die();
    }
}