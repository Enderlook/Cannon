using UnityEngine;

namespace Game.Ammunitions
{
    public class HurtOnHit : MonoBehaviour, IDamager
    {
        public float DamageMultiplier { get; private set; } = 1;

        public static void AddComponentTo(GameObject gameObject, float damageMultiplier)
        {
            HurtOnHit component = gameObject.AddComponent<HurtOnHit>();
            component.DamageMultiplier = damageMultiplier;
        }
    }
}