using UnityEngine;

namespace Game
{
    public class InstaKiller : MonoBehaviour
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Code Quality", "IDE0051:Remove unused private members", Justification = "Used by Unity.")]
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent(out IDamagable damagable))
                damagable.Kill();
        }
    }
}
